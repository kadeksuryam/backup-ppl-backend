using App.Models;

namespace App.Repositories
{
    public interface ITopUpHistoryRepository
    {
        Task<TopUpHistory> Add(TopUpHistory entity);
        Task<PagedList<TopUpHistory>> GetAll(PagingParameters getAllParameters);
        Task<IEnumerable<TopUpHistory>> GetAllByUserId(uint userId);
    }
}
