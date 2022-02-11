using if3250_2022_35_cakrawala_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace if3250_2022_35_cakrawala_backend.Data
{
    public interface IDataContext
    {
        DbSet<User> Users { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
