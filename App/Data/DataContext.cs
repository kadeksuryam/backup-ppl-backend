using App.Models;
using App.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace App.Data
{
    public class DataContext: DbContext, IDataContext
    {
        public DbSet<User> Users { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region Required
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);
        }
        #endregion
    }
}
