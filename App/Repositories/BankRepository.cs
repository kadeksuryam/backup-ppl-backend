using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class BankRepository : IBankRepository
    {
        private readonly IDataContext _context;
        public BankRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Bank?> GetById(uint id)
        {
            return await _context.Banks.Where(b => b.Id == id).FirstAsync();
        }

        public async Task<IEnumerable<Bank>> GetAll(List<uint> bankIds)
        {
            return await _context.Banks.Where(b => bankIds.Contains(b.Id)).ToListAsync();
        }

        public async Task<IEnumerable<Bank>> GetAll() {
            return await _context.Banks.ToListAsync();
        }

        public async Task<Bank> Add(Bank bank)
        {
            _context.Banks.Add(bank);
            await _context.SaveChangesAsync();
            return bank;
        }
    }
}
