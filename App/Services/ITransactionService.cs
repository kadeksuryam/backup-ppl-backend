using App.Models;
using App.DTOs.Requests;
using App.DTOs.Responses;

namespace App.Services
{
    public interface ITransactionService
    {
        Task<CreateTransactionResponseDTO> CreateTransaction(CreateTransactionRequestDTO spec);
        Task<GetTransactionHistoryResponseDTO> GetHistoryTransaction(PagingParameters getAllParam);
    }
}
