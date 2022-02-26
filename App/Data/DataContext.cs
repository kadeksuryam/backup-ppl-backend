using App.Models;
using App.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class DataContext: DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Level> Levels { get; set; }

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
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LevelEntityTypeConfiguration).Assembly);
        }
        #endregion
    }
}
