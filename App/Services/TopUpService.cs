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
        private readonly IUserRepository _userRepo;
        private readonly ITopUpHistoryRepository _historyRepo;
        private readonly IVoucherService _voucherService;
        private readonly IMapper _mapper;

        private Bank? SelectedBank;

        public TopUpService(IBankRepository bankRepo, IBankTopUpRequestRepository bankRequestRepo,
            IUserRepository userRepo, ITopUpHistoryRepository historyRepo,
            IVoucherService voucherService, IMapper mapper)
        {
            _bankRepo = bankRepo;
            _bankRequestRepo = bankRequestRepo;
            _userRepo = userRepo;
            _historyRepo = historyRepo;
            _voucherService = voucherService;
            _mapper = mapper;
        }

        public async Task<BankTopUpResponseDTO> BankTopUp(BankTopUpRequestDTO requestDto)
        {
            SelectedBank = await _bankRepo.GetById(requestDto.BankId);
            if (SelectedBank == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Bank ID is not valid.");
            }
            else
            {
                return await ExecuteBankTopUpRequestCreation(requestDto);
            }
        }

        private async Task<BankTopUpResponseDTO> ExecuteBankTopUpRequestCreation(BankTopUpRequestDTO requestDto)
        {
            BankTopUpRequest topUpRequest = CreateBankTopUpRequest(requestDto);
            await _bankRequestRepo.Add(topUpRequest);
            return CreateBankTopUpRequestDTO(topUpRequest);
        }

        private BankTopUpResponseDTO CreateBankTopUpRequestDTO(BankTopUpRequest topUpRequest)
        {
            BankTopUpResponseDTO response = _mapper.Map<BankTopUpResponseDTO>(topUpRequest);
            response.AccountNumber = SelectedBank!.AccountNumber;
            return response;
        }

        private BankTopUpRequest CreateBankTopUpRequest(BankTopUpRequestDTO requestDto)
        {
            BankTopUpRequest topUpRequest = _mapper.Map<BankTopUpRequest>(requestDto);
            topUpRequest.CreatedAt = DateTime.Now;
            topUpRequest.UpdatedAt = DateTime.Now;
            topUpRequest.ExpiredDate = DateTime.Now.AddDays(3); // Asumsikan pengguna diberi kesempatan 3 hari
            topUpRequest.Status = RequestStatus.Pending;
            return topUpRequest;
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

        public async Task<VoucherTopUpResponseDTO> VoucherTopUp(uint userId, VoucherTopUpRequestDTO request)
        {
            Voucher voucher = await _voucherService!.UseVoucher(request.VoucherCode);

            User? user = await _userRepo.GetById(userId);
            user!.Balance += voucher.Amount;

            TopUpHistory history = _mapper.Map<TopUpHistory>(voucher);
            history.FromUserId = userId;

            VoucherTopUpResponseDTO response = _mapper.Map<VoucherTopUpResponseDTO>(voucher);

            await _historyRepo.Add(history);
            await _userRepo.Update(user);

            return response;
        }

        public async Task<List<TopUpHistoryResponseDTO>> GetTopUpHistoriesByUser(uint userId)
        {
            IEnumerable<TopUpHistory> histories = await _historyRepo.GetAllByUserId(userId);
            histories = histories.OrderByDescending(history => history.UpdatedAt);

            List<TopUpHistoryResponseDTO> responses = new();
            foreach (TopUpHistory history in histories)
            {
                responses.Add(_mapper.Map<TopUpHistoryResponseDTO>(history));
            }

            return responses;
        }
    }
}
