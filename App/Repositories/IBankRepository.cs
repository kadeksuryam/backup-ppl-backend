using App.Models;

namespace App.Repositories
{
    public interface IBankRepository
    {
        Task<IEnumerable<Bank>> GetAll(List<uint> bankIds);
    }
}
