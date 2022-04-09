using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class LevelRepository : ILevelRepository
    {
        private readonly IDataContext _context;
        public LevelRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Level> GetById(uint id)
        {
            return await _context.Levels.Where(b => b.Id == id).FirstAsync();
        }

        public async Task<IEnumerable<Level>> GetAll()
        {
            return await _context.Levels.ToListAsync();
        }

        public async Task<Level?> GetByExp(uint exp)
        {
            return await _context.Levels
                .Where(b => b.RequiredExp <= exp)
                .OrderByDescending(b => b.RequiredExp)
                .FirstOrDefaultAsync();
        }
    }
}
