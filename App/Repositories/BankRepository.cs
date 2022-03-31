using App.Models;

namespace App.Repositories
{
    public class BankRepository : IBankRepository
    {
        public Task<Bank?> GetById(uint id)
        {
            throw new NotImplementedException();
        }
    }
}
