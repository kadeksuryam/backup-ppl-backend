using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Level> Levels { get; set; }
        DbSet<TopUpHistory> TopUpHistories { get; set; }
        DbSet<BankTopUpRequest> BankTopUpRequests { get; set; }
        DbSet<Bank> Banks { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
