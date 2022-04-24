using App.Data;
using App.Models;
using Microsoft.EntityFrameworkCore;

namespace App.Repositories
{
    public class VoucherRepository : IVoucherRepository
    {
        private readonly IDataContext _context;
        public VoucherRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<Voucher?> GetByCode(String code)
        {
            return await _context.Vouchers.Where(v => v.Code == code).FirstOrDefaultAsync();
        }

        public async Task<Voucher> Update(Voucher entity)
        {
            _context.Vouchers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Voucher> Add(Voucher entity)
        {
            _context.Vouchers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
