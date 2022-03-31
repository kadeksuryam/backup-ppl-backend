using App.DTOs.Responses;

namespace App.Services
{
    public interface IVoucherService
    {
        Task<GetVoucherResponseDTO> GetByCode(string code);
    }
}
