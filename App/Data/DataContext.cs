using App.Models;
using App.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class DataContext : DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<TopUpHistory> TopUpHistories { get; set; }

        public DbSet<BankTopUpRequest> BankTopUpRequests { get; set; }

        public DbSet<Bank> Banks { get; set; }


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            new LevelEntityTypeConfiguration().Configure(modelBuilder.Entity<Level>());
            new TopUpHistoryTypeConfiguration().Configure(modelBuilder.Entity<TopUpHistory>());
            new BankTopUpRequestTypeConfiguration().Configure(modelBuilder.Entity<BankTopUpRequest>());
            new BankTypeConfiguration().Configure(modelBuilder.Entity<Bank>());
            new VoucherEntityTypeConfiguration().Configure(modelBuilder.Entity<Voucher>());
        }
        #endregion
    }
}
