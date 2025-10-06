using Application.Repositories;
using Domain.Entities;

namespace Infraestructure.Repository;

public class UserRepository : IUserRepository
{


    public Task<int> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<User>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<User?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveAsync(User model)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(User model)
    {
        throw new NotImplementedException();
    }
}
