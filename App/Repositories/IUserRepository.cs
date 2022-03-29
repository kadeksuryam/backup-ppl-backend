using App.Models;

namespace App.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<IEnumerable<User>> GetAll(List<uint> userIds);
        Task<User> GetById(uint id);
        Task<User> Add(User entity);
        Task<User> Update(User entity);
        Task<User?> Delete(int id);
        Task<User?> Get(int id);
        Task<User?> GetByUsername(string username);
        Task<User?> GetByEmail(string email);
    }
}
