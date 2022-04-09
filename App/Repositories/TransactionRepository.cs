using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IDataContext _context;
        public TransactionRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<TransactionHistory> Add(TransactionHistory entity)
        {
            _context.TransactionHistories.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TransactionHistory> Update(TransactionHistory entity)
        {
            _context.TransactionHistories.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TransactionHistory?> Delete(int id)
        {
            var entity = await _context.TransactionHistories.FindAsync(id);
            if (entity != null)
            {
                _context.TransactionHistories.Remove(entity);
                await _context.SaveChangesAsync();
            }

            return entity;
        }

        public async Task<TransactionHistory?> Get(int id)
        {
            return await _context.TransactionHistories.Where(b => b.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TransactionHistory>> GetAllByUserId(uint userId)
        {
            return await _context.TransactionHistories
                .Where(history => history.FromUserId == userId)
                .ToListAsync();
        }
    }
}
