using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using AutoMapper;
using System.Net;

namespace App.Services
{
    public class BankTopUpService : IBankTopUpService
    {
        private readonly IBankRepository _bankRepo;
        private readonly IBankTopUpRequestRepository _bankRequestRepo;
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;

        public BankTopUpService(IBankRepository bankRepo, IBankTopUpRequestRepository bankRequestRepo, IUserRepository userRepo, IMapper mapper)
        {
            _bankRepo = bankRepo;
            _bankRequestRepo = bankRequestRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }
        public async Task<BankTopUpResponseDTO> BankTopUp(uint userId, BankTopUpRequestDTO request)
        {
            Bank? bank = await _bankRepo.GetById(request.BankId);
            if (bank == null)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "Bank ID is not valid.");
            }
            else
            {
                User user = await _userRepo.GetById(userId);
                

                BankTopUpResponseDTO response = _mapper.Map<BankTopUpResponseDTO>(user);
                response.AccountNumber = bank.AccountNumber;
                response.ExpiredDate = DateTime.Now.AddDays(3).ToString("dd/MM/yyyy, HH.mm");
                return response;
            }
        }
    }
}