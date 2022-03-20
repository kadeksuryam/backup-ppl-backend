using App.Models;

namespace App.Repositories
{
    public interface ILevelRepository
    {
        Task<IEnumerable<Level>> GetAll();
        Task<Level> GetById(uint id);
    }
}
