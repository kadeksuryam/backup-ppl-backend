using App.Models;

namespace App.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(uint id);
        Task<User> Add(User entity);
        Task<User> Update(User entity);
        Task<User> Delete(int id);
        Task<User> Get(string username);
        Task<User> GetByEmail(string email);
    }
}
