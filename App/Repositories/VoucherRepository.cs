﻿using App.Data;
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
    }
}
