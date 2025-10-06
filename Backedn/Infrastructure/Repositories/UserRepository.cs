using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<User> AddAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return user;
    }

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        // Devuelve la entidad rastreada para permitir actualizaciones posteriores.
        return await _context.Users.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        // Usa AsNoTracking para mejorar rendimiento en lecturas.
        return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));

        var entry = _context.Entry(user);
        if (entry.State == EntityState.Detached)
        {
            // Si la entidad no está siendo rastreada, se adjunta y se marca como modificada.
            _context.Users.Attach(user);
            entry = _context.Entry(user);
            entry.State = EntityState.Modified;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(User user, CancellationToken cancellationToken = default)
    {
        if (user is null) throw new ArgumentNullException(nameof(user));

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
