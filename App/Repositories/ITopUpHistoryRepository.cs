using App.Models;

namespace App.Repositories
{
    public interface ITopUpHistoryRepository
    {
        Task<TopUpHistory> Add(TopUpHistory entity);
    }
}
