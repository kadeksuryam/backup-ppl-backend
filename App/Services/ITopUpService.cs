using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Models;
using App.Models.Enums;

namespace App.Services
{
    public interface ITopUpService
    {
        Task<BankTopUpResponseDTO> BankTopUp(BankTopUpRequestDTO request);
        Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus);
        Task<VoucherTopUpResponseDTO> VoucherTopUp(VoucherTopUpRequestDTO request);
        Task<List<TopUpHistoryResponseDTO>> GetTopUpHistoriesByUser(uint userId);
        Task UpdateBankTopUpRequest(UpdateTopUpRequestStatusRequestDTO dto);
        Task<GetTopUpHistoryResponseDTO> GetHistoryTransaction(PagingParameters getAllParam);
    }
}
