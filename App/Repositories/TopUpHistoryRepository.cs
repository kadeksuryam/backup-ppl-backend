using App.Data;
using App.Models;

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
    }
}
