using Domain.Entities;

namespace Application.Repositories;

public interface IUserRepository
{
    Task<int> SaveAsync(User model);
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<int> UpdateAsync(User model);
    Task<int> DeleteAsync(int id);
}
