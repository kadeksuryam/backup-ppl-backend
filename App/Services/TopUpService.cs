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
        private readonly IBankTopUpRequestRepository _bankTopUpRequestRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITopUpHistoryRepository _topUpHistoryRepository;
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public TopUpService(IBankTopUpRequestRepository bankTopUpRequestRepository, 
            IMapper mapper, IUserRepository userRepository, 
            ITopUpHistoryRepository topUpHistoryRepository, DataContext context)
        {
            _bankTopUpRequestRepository = bankTopUpRequestRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _topUpHistoryRepository = topUpHistoryRepository;
            _dataContext = context;
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
                } catch(Exception ex)
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
            } catch (Exception ex)
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
                    CreatedAt = history.CreatedAt.ToString("o"),
                    UpdatedAt = history.CreatedAt.ToString("o"),
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
