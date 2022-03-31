using App.Data;
using App.Models;

namespace App.Repositories
{
    public class BankTopUpRequestRepository : IBankTopUpRequestRepository
    {
        private readonly IDataContext _context;
        public BankTopUpRequestRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<BankTopUpRequest> Add(BankTopUpRequest request)
        {
            _context.BankTopUpRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }

        public Task<IEnumerable<BankTopUpRequest>> GetAllPending()
        {
            throw new NotImplementedException();
        }
    }
}
