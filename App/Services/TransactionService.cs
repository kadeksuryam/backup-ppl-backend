using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using AutoMapper;
using System.Net;

namespace App.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public TransactionService(ITransactionRepository transactionRepository, 
            IUserRepository userRepository, IMapper mapper)
        {
            _transactionRepository = transactionRepository;
            _userRepository = userRepository;
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

            await _userRepository.Update(user);
            await _userRepository.Update(targetUser);

            return response;
        }
    }
}
