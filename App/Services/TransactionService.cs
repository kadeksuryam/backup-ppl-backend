using App.Data;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;
using static App.DTOs.Responses.GetTransactionHistoryResponseDTO;

namespace App.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IDataContext _context;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
<<<<<<< HEAD
        public TransactionService(IDataContext context, ITransactionRepository transactionRepository, 
            IUserRepository userRepository, IUserService userService, IMapper mapper)
=======
        public TransactionService(IDataContext context, ITransactionRepository transactionRepository,
            IUserRepository userRepository, IMapper mapper)
>>>>>>> main
        {
            _context = context;
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<CreateTransactionResponseDTO> CreateTransaction(CreateTransactionRequestDTO dto)
        {
            if (dto.Amount <= 0)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Amount must be greater than 0");
            }

            var user = await _userRepository.GetById(dto.FromUserId);
            if (user == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Sender user with ID '" + dto.FromUserId + "' is not found");
            }
            if (user.Balance < dto.Amount)
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Insufficient balance");
            }

            var targetUser = await _userRepository.GetById(dto.ToUserId);
            if (targetUser == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.NotFound, "Target user with ID '" + dto.ToUserId + "' is not found");
            }

            var transaction = _mapper.Map<TransactionHistory>(dto);

            DateTime dateTime = DateTime.Now;

            transaction.CreatedAt = dateTime;
            transaction.UpdatedAt = dateTime;
            transaction.Status = TransactionHistory.TransactionStatus.Success;

            using (IDbContextTransaction t = _context.BeginTransaction())
            {
                try
                {
                    await _transactionRepository.Add(transaction);

                    uint currentUserBalance = user.Balance - dto.Amount;
                    uint currentTargetUserBalance = targetUser.Balance + dto.Amount;

                    var response = _mapper.Map<CreateTransactionResponseDTO>(transaction);

                    response.From!.PreviousBalance = user.Balance;
                    response.From!.CurrentBalance = currentUserBalance;
                    response.To!.PreviousBalance = targetUser.Balance;
                    response.To!.CurrentBalance = currentTargetUserBalance;

                    user.Balance = currentUserBalance;
                    targetUser.Balance = currentTargetUserBalance;

                    await _userRepository.Update(targetUser);
                    await _userRepository.Update(user);

                    await _userService.AddExp(user, dto.Amount / 10000);

                    t.Commit();

                    return response;
                } 
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    t.Rollback();
                    throw new HttpStatusCodeException(HttpStatusCode.InternalServerError, "An error occured");
                }
            }
        }

        public async Task<List<TransactionHistoryResponseDTO>> GetTransactionHistoriesByUser(uint userId)
        {
            IEnumerable<TransactionHistory> histories = await _transactionRepository.GetAllByUserId(userId);
            histories = histories.OrderByDescending(history => history.UpdatedAt);

            List<TransactionHistoryResponseDTO> responses = new();
            foreach (TransactionHistory history in histories)
            {
                responses.Add(_mapper.Map<TransactionHistoryResponseDTO>(history));
            }

            return responses;
        }

        public async Task<GetTransactionHistoryResponseDTO> GetHistoryTransaction(PagingParameters getAllParam)
        {

            GetTransactionHistoryResponseDTO res = new GetTransactionHistoryResponseDTO();
            res.TransactionHistories = new List<TransactionHistoryDTO>();

            PagedList<TransactionHistory> histories = await _transactionRepository.GetAll(getAllParam);



            foreach (var history in histories)
            {
                System.Console.WriteLine(history);
                TransactionHistoryDTO dto = new TransactionHistoryDTO()
                {
                    Id = history.Id,
                    CreatedAt = history.CreatedAt.ToString("o"),
                    UpdatedAt = history.CreatedAt.ToString("o"),
                    From = _mapper.Map<UserDTO>(history.From),
                    To = _mapper.Map<UserDTO>(history.To),
                    Amount = history.Amount,
                    Status = history.Status.ToString()
                };
                res.TransactionHistories.Add(dto);
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
