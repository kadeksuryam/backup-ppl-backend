using App.Data;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Models.Enums;
using App.Repositories;
using AutoMapper;
using System.Net;
using static App.DTOs.Responses.GetTopUpHistoryResponseDTO;

namespace App.Services
{
    public class TopUpService : ITopUpService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IBankTopUpRequestRepository _bankTopUpRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;
        private readonly IVoucherService _voucherService;
        private readonly IMapper _mapper;
        private readonly DataContext _dataContext;
        private Bank? SelectedBank;

        public TopUpService(IBankRepository bankRepository,
            IBankTopUpRequestRepository bankTopUpRequestRepository,
            IUserRepository userRepository,
            ITopUpHistoryRepository topUpHistoryRepository,
            IVoucherService voucherService,
            IMapper mapper, DataContext context)
        {
            _bankRepository = bankRepository;
            _bankTopUpRequestRepository = bankTopUpRequestRepository;
            _voucherService = voucherService;
            _userRepository = userRepository;
            _mapper = mapper;
            _topUpHistoryRepository = topUpHistoryRepository;
            _dataContext = context;
        }

        public async Task<BankTopUpResponseDTO> BankTopUp(BankTopUpRequestDTO requestDto)
        {
            SelectedBank = await _bankRepository.GetById(requestDto.BankId);
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
            await _bankTopUpRequestRepository.Add(topUpRequest);
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
            IEnumerable<BankTopUpRequest> requests = await _bankTopUpRequestRepository.GetAll(requestStatus);
            List<GetBankTopUpRequestResponseDTO> response = new List<GetBankTopUpRequestResponseDTO>();

            foreach (var request in requests)
            {
                GetBankTopUpRequestResponseDTO dto = new GetBankTopUpRequestResponseDTO()
                {
                    Id = request.Id,
                    CreatedAt = request.CreatedAt,
                    UpdatedAt = request.UpdatedAt,
                    ExpiredDate = request.ExpiredDate,
                    Amount = request.Amount,
                    Bank = _mapper.Map<GetBankTopUpRequestResponseDTO.BankDTO>(request.Bank),
                    From = _mapper.Map<GetBankTopUpRequestResponseDTO.UserDTO>(request.From),
                    Status = request.Status.ToString()
                };
                response.Add(dto);
            }
            return response;
        }
        public async Task<VoucherTopUpResponseDTO> VoucherTopUp(VoucherTopUpRequestDTO request)
        {
            Voucher voucher = await _voucherService!.UseVoucher(request.VoucherCode);

            User? user = await _userRepository.GetById(request.UserId);
            user!.Balance += voucher.Amount;

            TopUpHistory history = _mapper.Map<TopUpHistory>(voucher);
            history.FromUserId = request.UserId;

            VoucherTopUpResponseDTO response = _mapper.Map<VoucherTopUpResponseDTO>(voucher);

            await _topUpHistoryRepository.Add(history);
            await _userRepository.Update(user);

            return response;
        }

        public async Task<List<TopUpHistoryResponseDTO>> GetTopUpHistoriesByUser(uint userId)
        {
            IEnumerable<TopUpHistory> histories = await _topUpHistoryRepository.GetAllByUserId(userId);
            histories = histories.OrderByDescending(history => history.UpdatedAt);

            List<TopUpHistoryResponseDTO> responses = new();
            foreach (TopUpHistory history in histories)
            {
                responses.Add(_mapper.Map<TopUpHistoryResponseDTO>(history));
            }

            return responses;
        }

        public async Task UpdateBankTopUpRequest(UpdateTopUpRequestStatusRequestDTO dto)
        {
            BankTopUpRequest? bankTopUpRequest = (await _bankTopUpRequestRepository.Get(dto.Id));
            RequestStatus? requestStatus = (RequestStatus?)Enum.Parse(typeof(RequestStatus), dto.Status!);
            User? userDb = bankTopUpRequest != null ? bankTopUpRequest!.From : null;
            Bank? bankDb = bankTopUpRequest != null ? bankTopUpRequest!.Bank : null;

            if (bankTopUpRequest == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Invalid TopUp Request Id");
            }
            if (!bankTopUpRequest.Status.Equals(RequestStatus.Pending))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Invalid request status");
            }

            using var transaction = _dataContext.Database.BeginTransaction();
            if (bankTopUpRequest.ExpiredDate.ToUniversalTime() < DateTime.UtcNow)
            {
                try
                {
                    bankTopUpRequest.Status = RequestStatus.Failed;
                    bankTopUpRequest.UpdatedAt = DateTime.UtcNow;
                    await _bankTopUpRequestRepository.Update(bankTopUpRequest);

                    _dataContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
                }
                throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "TopUp Request Expired");
            }

            try
            {

                bankTopUpRequest.Status = requestStatus;
                bankTopUpRequest.UpdatedAt = DateTime.UtcNow;
                if (requestStatus.Equals(RequestStatus.Success))
                {
                    userDb!.Balance += (uint)bankTopUpRequest.Amount;
                    // increase the EXP?
                    userDb!.Exp += 100;
                }
                TopUpHistory topUpHistory = new()
                {
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    Amount = bankTopUpRequest.Amount,
                    Method = TopUpHistory.TopUpMethod.Bank,
                    FromUserId = userDb!.Id,
                    BankRequestId = bankDb!.Id,
                    VoucherId = null
                };
                await _topUpHistoryRepository.Add(topUpHistory);
                await _bankTopUpRequestRepository.Update(bankTopUpRequest);
                await _userRepository.Update(userDb);

                _dataContext.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        public async Task<GetTopUpHistoryResponseDTO> GetHistoryTransaction(PagingParameters getAllParam)
        {

            GetTopUpHistoryResponseDTO res = new GetTopUpHistoryResponseDTO();
            res.TopUpHistories = new List<TopUpHistoryDTO>();

            PagedList<TopUpHistory> histories = await _topUpHistoryRepository.GetAll(getAllParam);



            foreach (var history in histories)
            {
                Console.WriteLine(history.BankRequest != null);
                TopUpHistoryDTO dto = new TopUpHistoryDTO()
                {
                    Id = history.Id,
                    CreatedAt = history.CreatedAt,
                    UpdatedAt = history.CreatedAt,
                    From = _mapper.Map<UserDTO>(history.From),
                    Amount = (uint)history.Amount,
                    Method = history.Method.ToString(),
                    Voucher = _mapper.Map<VoucherDTO>(history.Voucher),
                    Bank = history.BankRequest != null ? _mapper.Map<BankDTO>(history.BankRequest.Bank) : null
                };
                res.TopUpHistories.Add(dto);
            }

            res.Paging = new PagingDTO()
            {
                page = histories.CurrentPage,
                size = histories.PageSize,
                count = histories.Count,
            };

            return res;
        }
    }
}
