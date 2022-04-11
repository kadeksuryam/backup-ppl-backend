using App.Data;
using App.Models;
using App.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class BankTopUpRequestRepository : IBankTopUpRequestRepository
    {
        private readonly IDataContext _context;
        public BankTopUpRequestRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<BankTopUpRequest>> GetAll(RequestStatus? requestStatus)
        {
            return await _context.BankTopUpRequests
                .Where(b => b.Status.Equals(requestStatus))
                .Include(b => b.Bank)
                .Include(b => b.From)
                .ToListAsync();
        }

        public async Task<BankTopUpRequest> Update(BankTopUpRequest entity)
        {
            _context.BankTopUpRequests.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<BankTopUpRequest?> Get(uint id)
        {
            return await _context.BankTopUpRequests
                .Where(b => b.Id == id)
                .Include(b => b.Bank)
                .Include(b => b.From)
                .FirstAsync(); 
        }
    }
}
