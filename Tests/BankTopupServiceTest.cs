using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using App.Services;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class BankTopupServiceTest
    {
        private Mock<IBankRepository>? mockBankRepo;
        private Bank? mockBank;
        public Mock<IUserRepository>? mockUserRepo;
        private User? user;
        private Mock<IBankTopUpRequestRepository>? mockBankRequestRepo;

        [Fact]
        public async Task BankTopUp_ValidRequest_ReturnsSuccessAsync()
        {
            uint userId = GetAuthenticatedUserId();
            BankTopUpRequestDTO request = GetValidBankTopUpRequest();
            BankTopUpResponseDTO response = await MakeBankTopUp(userId, request);
            AssertValidBankTopUpResponse(response);
            AssertExactlyOneBankTopUpRequestAdded();
        }

        private uint GetAuthenticatedUserId()
        {
            uint userId = 1234;
            MakeUserIdAuthenticated(userId);
            return userId;
        }

        private void MakeUserIdAuthenticated(uint userId)
        {
            mockUserRepo = new();
            user = CreateUser(userId);
            mockUserRepo.Setup(repo => repo.GetById(userId)).ReturnsAsync(user);
        }

        private User CreateUser(uint userId)
        {
            User user = new();
            user.Id = userId;
            return user;
        }

        private BankTopUpRequestDTO GetValidBankTopUpRequest()
        {
            BankTopUpRequestDTO request = new();
            request.Amount = 50000;
            request.BankId = 6789;

            MakeRequestValid(request);
            return request;
        }

        private void MakeRequestValid(BankTopUpRequestDTO request)
        {
            mockBank = new();
            mockBank.Id = request.BankId;

            mockBankRepo = new();
            mockBankRepo.Setup(repo => repo.GetById(request.BankId)).ReturnsAsync(mockBank);
        }

        private async Task<BankTopUpResponseDTO> MakeBankTopUp(uint userId, BankTopUpRequestDTO request)
        {
            return await GetBankTopUpService().BankTopUp(userId, request);
        }

        private BankTopUpService GetBankTopUpService()
        {
            Mapper mapper = CreateMapper();
            mockBankRequestRepo = new();

            return new BankTopUpService(
                mockBankRepo!.Object,
                mockBankRequestRepo.Object,
                mockUserRepo!.Object,
                mapper
            );
        }

        private Mapper CreateMapper()
        {
            MapperConfiguration mapperConfig = new(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });
            return new Mapper(mapperConfig);
        }

        private void AssertValidBankTopUpResponse(BankTopUpResponseDTO response)
        {
            AssertAccountNumberMatch(response.AccountNumber);
            AssertParseableToDateTime(response.ExpiredDate);
        }

        private void AssertAccountNumberMatch(long actualAccountNumber)
        {
            Assert.Equal(mockBank!.AccountNumber, actualAccountNumber);
        }

        private void AssertParseableToDateTime(string expiredDate)
        {
            Assert.True(DateTime.TryParse(expiredDate, out _));
        }

        private void AssertExactlyOneBankTopUpRequestAdded()
        {
            mockBankRequestRepo!.Verify(
                repo => repo.Add(It.IsAny<BankTopUpRequest>()),
                Times.Once
            );
        }
    }
}
