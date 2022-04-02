using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models.Enums;

namespace App.Services
{
    public interface ITopUpService
    {
        Task<BankTopUpResponseDTO> BankTopUp(uint userId, BankTopUpRequestDTO request);
        Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus);
        Task<VoucherTopUpResponseDTO> VoucherTopUp(uint userId, VoucherTopUpRequestDTO request);
    }
}
