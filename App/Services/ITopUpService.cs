using App.DTOs.Responses;
﻿using App.DTOs.Requests;
using App.Models;
using App.Models.Enums;

namespace App.Services
{
    public interface ITopUpService
    {
        Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus);
        Task UpdateBankTopUpRequest(UpdateTopUpRequestStatusRequestDTO dto);
        Task<GetTopUpHistoryResponseDTO> GetHistoryTransaction(PagingParameters getAllParam);
    }
}
