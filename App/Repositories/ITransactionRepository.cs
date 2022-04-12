using App.Models;

namespace App.Repositories
{
    public interface ITransactionRepository
    {
        Task<TransactionHistory> Add(TransactionHistory entity);
        Task<TransactionHistory> Update(TransactionHistory entity);
        Task<TransactionHistory?> Delete(int id);
        Task<TransactionHistory?> Get(int id);
        Task<PagedList<TransactionHistory>> GetAll(PagingParameters getAllParameters);
    }
}
