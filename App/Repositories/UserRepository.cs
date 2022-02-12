﻿using if3250_2022_35_cakrawala_backend.Data;
using if3250_2022_35_cakrawala_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace if3250_2022_35_cakrawala_backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDataContext _context;
        public UserRepository(IDataContext context)
        {
            this._context = context;
        }
        public async Task Add(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> Get(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task Update(User user)
        {
            var itemToUpdate = await _context.Users.FindAsync(user.Id);
            if(itemToUpdate == null)
            {
                throw new NullReferenceException();
            }

            itemToUpdate.DisplayName = user.DisplayName;
            await _context.SaveChangesAsync();
        }
    }
}
