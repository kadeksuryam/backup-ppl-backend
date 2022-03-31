using App.DTOs.Responses;
using App.Models;

namespace App.Services
{
    public interface IVoucherService
    {
        Task<GetVoucherResponseDTO> GetByCode(string code);

        Task<Voucher> UseVoucher(string code);
    }
}
