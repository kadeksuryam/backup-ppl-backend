using App.Models;

namespace App.Repositories
{
    public interface ITopUpHistoryRepository
    {
        Task<TopUpHistory> Add(TopUpHistory entity);
        Task<IEnumerable<TopUpHistory>> GetAllByUserId(uint userId);
    }
}
