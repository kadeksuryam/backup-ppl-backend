﻿using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}