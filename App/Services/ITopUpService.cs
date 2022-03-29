using App.DTOs.Responses;
using App.Models.Enums;

namespace App.Services
{
    public interface ITopUpService
    {
        Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus);
    }
}
