using App.Models;

namespace App.Repositories
{
    public class BankTopUpRequestRepository : IBankTopUpRequestRepository
    {
        public Task<BankTopUpRequest> Add(BankTopUpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<BankTopUpRequest>> GetAllPending()
        {
            throw new NotImplementedException();
        }
    }
}
