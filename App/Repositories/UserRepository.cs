using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User entity)
        {
            _context.Users.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> Delete(int id)
        {
            var entity = await _context.Users.FindAsync(id);
            if (entity == null)
            {
                return entity;
            }

            _context.Users.Remove(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.Where(b => b.Id == id).FirstAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> Update(User entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // We can add new methods specific to the user repository here in the future
        public async Task<User> Get(string username)
        {
            return await _context.Users.FindAsync(username);
        }
    }
}
