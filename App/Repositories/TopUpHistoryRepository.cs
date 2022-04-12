using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;
namespace App.Repositories
{
    public class TopUpHistoryRepository : ITopUpHistoryRepository
    {
        private readonly IDataContext _context;
        public TopUpHistoryRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TopUpHistory> Add(TopUpHistory entity)
        {
            _context.TopUpHistories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PagedList<TopUpHistory>> GetAll(PagingParameters getAllParameters)
        {
            return await PagedList<TopUpHistory>.ToPagedListAsync(
                _context.TopUpHistories
                .Include(b => b.From)
                .Include(b => b.BankRequest)
                .Include(b => b.BankRequest != null ? b.BankRequest.Bank : null)
                .Include(b => b.Voucher)
                .OrderBy(b => b.Id), getAllParameters);
        }
    }
}
