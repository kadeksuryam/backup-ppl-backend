using App.Models;

namespace App.Repositories
{
    public interface IVoucherRepository
    {
        Task<Voucher?> GetByCode(String code);
    }
}
