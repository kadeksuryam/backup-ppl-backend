using System;
using System.Net;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using AutoMapper;

namespace App.Services
{
    public class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IMapper _mapper;

        public BankService(IBankRepository bankRepository, IMapper mapper)
        {
            _bankRepository = bankRepository;
            _mapper = mapper;
        }

        public async Task AddNewBank(AddBankRequestDTO dto)
        {
            IEnumerable<Bank> dbBanks = await _bankRepository.GetAll();

            bool isUniqueBankAccount = true;
            foreach (Bank dbBank in dbBanks)
            {
               if(dbBank.Name!.Equals(dto.Name) && dbBank.AccountNumber.Equals(dto.AccountNumber))
               {
                    isUniqueBankAccount = false;
                    break;
               }
            }

            if(!isUniqueBankAccount)
            {
                throw new HttpStatusCodeException(HttpStatusCode.UnprocessableEntity, "Bank account must be unique!");
            }


            Bank bank = _mapper.Map<Bank>(dto);
            await _bankRepository.Add(bank);
        }


        public async Task<GetAllBankResponseDTO> GetAllBank()
        {

            IEnumerable<Bank> dbBanks = await _bankRepository.GetAll();
            GetAllBankResponseDTO resDTO = new GetAllBankResponseDTO();
            resDTO.Banks = new List<GetAllBankResponseDTO.BankDTO>();

            foreach(Bank bank in dbBanks)
            {
                resDTO.Banks.Add(_mapper.Map<GetAllBankResponseDTO.BankDTO>(bank));
            }

            return resDTO;
        }
    }
}

