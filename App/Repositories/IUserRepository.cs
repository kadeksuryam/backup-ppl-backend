using if3250_2022_35_cakrawala_backend.Models;

namespace if3250_2022_35_cakrawala_backend.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(int id);
        Task<IEnumerable<User>> GetAll();
        Task Add(User user);
        Task Update(User user);
    }
}
