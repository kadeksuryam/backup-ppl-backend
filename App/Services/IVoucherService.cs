using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models;

namespace App.Services
{
    public interface IVoucherService
    {
        Task<GetVoucherResponseDTO> GetByCode(string code);

        Task<Voucher> UseVoucher(string code);

        Task AddVoucher(AddVoucherRequestDTO dto);
    }
}
