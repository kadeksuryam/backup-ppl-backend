using App.Models;
using App.Models.Enums;

namespace App.Repositories
{
    public interface IBankTopUpRequestRepository
    {
        Task<BankTopUpRequest> Add(BankTopUpRequest request);
        Task<IEnumerable<BankTopUpRequest>> GetAllPending();
        Task<IEnumerable<BankTopUpRequest>> GetAll(RequestStatus? requestStatus);
        Task<BankTopUpRequest> Update(BankTopUpRequest entity);
        Task<BankTopUpRequest?> Get(uint id);
    }
}
