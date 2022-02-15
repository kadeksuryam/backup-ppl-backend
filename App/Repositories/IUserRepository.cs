using App.Models;

namespace App.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> Get(int id);
        Task<User> Add(User entity);
        Task<User> Update(User entity);
        Task<User> Delete(int id);
        Task<User> Get(string username);
    }
}
