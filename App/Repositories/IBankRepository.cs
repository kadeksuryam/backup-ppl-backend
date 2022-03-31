using App.Models;

namespace App.Repositories
{
    public interface IBankRepository
    {
        Task<Bank?> GetById(uint id);
    }
}
