using App.Models;
using App.Models.Enums;

namespace App.Repositories
{
    public interface IBankTopUpRequestRepository
    {
        Task<IEnumerable<BankTopUpRequest>> GetAll(RequestStatus? requestStatus);
        Task<BankTopUpRequest> Update(BankTopUpRequest entity);
        Task<BankTopUpRequest?> Get(uint id);
    }
}
