using App.DTOs.Requests;
using App.DTOs.Responses;

namespace App.Services
{
    public interface ITopUpService
    {
        Task<BankTopUpResponseDTO> BankTopUp(uint userId, BankTopUpRequestDTO request);
    }
}
