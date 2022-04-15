using App.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        DbSet<Level> Levels { get; set; }
        DbSet<BankTopUpRequest> BankTopUpRequests { get; set; }
        DbSet<TopUpHistory> TopUpHistories { get; set; }
        DbSet<TransactionHistory> TransactionHistories { get; set; }
        DbSet<Bank> Banks { get; set; }
        DbSet<Voucher> Vouchers { get; set; }
        IDbContextTransactionProxy BeginTransaction();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
