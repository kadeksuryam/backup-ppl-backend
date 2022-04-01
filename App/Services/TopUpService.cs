using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Models.Enums;
using App.Repositories;
using AutoMapper;
using System.Net;

namespace App.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly IBankRepository _bankRepo;
        private readonly IBankTopUpRequestRepository _bankRequestRepo;
        private readonly IMapper _mapper;

        private Bank? SelectedBank;

        public TopUpService(IBankRepository bankRepo, IBankTopUpRequestRepository bankRequestRepo, IMapper mapper)
        {
            _bankRepo = bankRepo;
            _bankRequestRepo = bankRequestRepo;
            _mapper = mapper;
        }
        public async Task<BankTopUpResponseDTO> BankTopUp(uint userId, BankTopUpRequestDTO requestDto)
        {
            SelectedBank = await _bankRepo.GetById(requestDto.BankId);
            if (SelectedBank == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Bank ID is not valid.");
            }
            else
            {
                return await ExecuteBankTopUpRequestCreation(userId, requestDto);
            }
        }

        private async Task<BankTopUpResponseDTO> ExecuteBankTopUpRequestCreation(uint userId, BankTopUpRequestDTO requestDto)
        {
            BankTopUpRequest topUpRequest = CreateBankTopUpRequest(userId, requestDto);
            await _bankRequestRepo.Add(topUpRequest);
            return CreateBankTopUpRequestDTO(topUpRequest);
        }

        private BankTopUpResponseDTO CreateBankTopUpRequestDTO(BankTopUpRequest topUpRequest)
        {
            BankTopUpResponseDTO response = _mapper.Map<BankTopUpResponseDTO>(topUpRequest);
            response.AccountNumber = SelectedBank!.AccountNumber;
            return response;
        }

        private BankTopUpRequest CreateBankTopUpRequest(uint userId, BankTopUpRequestDTO requestDto)
        {
            BankTopUpRequest topUpRequest = _mapper.Map<BankTopUpRequest>(requestDto);
            topUpRequest.CreatedAt = DateTime.Now;
            topUpRequest.UpdatedAt = DateTime.Now;
            topUpRequest.ExpiredDate = DateTime.Now.AddDays(3); // Asumsikan pengguna diberi kesempatan 3 hari
            topUpRequest.FromUserId = userId;
            topUpRequest.Status = RequestStatus.Pending;
            return topUpRequest;
        }

        public TopUpService(IBankTopUpRequestRepository bankTopUpRequestRepository, IMapper mapper)
        {
            _bankRequestRepo = bankTopUpRequestRepository;
            _mapper = mapper;
        }

        public async Task<List<GetBankTopUpRequestResponseDTO>> GetBankTopUpRequest(RequestStatus? requestStatus)
        {
            IEnumerable<BankTopUpRequest> requests = await _bankRequestRepo.GetAll(requestStatus);
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
