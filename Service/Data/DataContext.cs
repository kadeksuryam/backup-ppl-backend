using if3250_2022_35_cakrawala_backend.Models;
using if3250_2022_35_cakrawala_backend.Models.Config;
using Microsoft.EntityFrameworkCore;

namespace if3250_2022_35_cakrawala_backend.Data
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
