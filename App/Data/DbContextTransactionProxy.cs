using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace App.Data
{
    public class DbContextTransactionProxy : IDbContextTransactionProxy
    {
        /// <summary>
        /// Real Class which we want to control.
        /// We can't mock it's because it does not have public constructors.
        /// </summary>
        private readonly IDbContextTransaction _transaction;

        public DbContextTransactionProxy(DbContext context)
        {
            _transaction = context.Database.BeginTransaction();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }
    }
}
