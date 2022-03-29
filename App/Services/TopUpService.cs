using App.DTOs.Responses;
using App.Models;
using App.Models.Enums;
using App.Repositories;
using AutoMapper;

namespace App.Services
{
    public class TopUpService : ITopUpService

    {
        private readonly IBankTopUpRequestRepository _bankTopUpRequestRepository;
        private readonly IMapper _mapper;

        public TopUpService(IBankTopUpRequestRepository bankTopUpRequestRepository, IMapper mapper)
        {
            _bankTopUpRequestRepository = bankTopUpRequestRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus)
        {
            IEnumerable<BankTopUpRequest> requests = await _bankTopUpRequestRepository.GetAll(requestStatus);
            List<GetBankTopUpRequestResponseDTO> response = new List<GetBankTopUpRequestResponseDTO>();
           
            foreach (var request in requests)
            {
                GetBankTopUpRequestResponseDTO dto = new GetBankTopUpRequestResponseDTO()
                {
                    Id = request.Id,
                    CreatedAt = request.CreatedAt.ToString("o"),
                    UpdatedAt = request.UpdatedAt.ToString("o"),
                    ExpiredDate = request.ExpiredDate.ToString("o"),
                    Amount = request.Amount,
                    Bank = _mapper.Map<GetBankTopUpRequestResponseDTO.BankDTO>(request.Bank),
                    From = _mapper.Map<GetBankTopUpRequestResponseDTO.UserDTO>(request.From),
                    Status = request.Status.ToString()
                };
                response.Add(dto);
            }
            return response;
        }
    }
}
