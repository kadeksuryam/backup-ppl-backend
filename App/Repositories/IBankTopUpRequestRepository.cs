using App.Models;

namespace App.Repositories
{
    public interface IBankTopUpRequestRepository
    {
        Task<BankTopUpRequest> Add(BankTopUpRequest request);
        Task<IEnumerable<BankTopUpRequest>> GetAllPending();
    }
}
