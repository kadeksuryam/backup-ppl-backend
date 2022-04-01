using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Models.Enums;
using App.Repositories;
using App.Services;
using AutoMapper;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class TopUpServiceTest
    {
        private Mock<IBankRepository>? mockBankRepo;
        private Bank? mockBank;
        private Mock<IUserRepository>? mockUserRepo;
        private Mock<IBankTopUpRequestRepository>? mockBankRequestRepo;
        private MapperConfiguration? mapperConfig;
        private Mapper? mapper;
        private TopUpService? topUpService;

        private void Initialize()
        {
            mockBankRequestRepo = new Mock<IBankTopUpRequestRepository>();
            mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            mapper = new Mapper(mapperConfig);
            topUpService = new TopUpService(mockBankRequestRepo.Object, mapper);
        }

        [Fact]
        public async Task BankTopUp_ValidRequest_ReturnsSuccessAsync()
        {
            uint userId = 1234;
            BankTopUpRequestDTO request = GetValidBankTopUpRequest();
            BankTopUpResponseDTO response = await MakeBankTopUp(userId, request);
            AssertValidBankTopUpResponse(response);
            AssertExactlyOneBankTopUpRequestAdded();
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
            return await GetTopUpService().BankTopUp(userId, request);
        }

        private TopUpService GetTopUpService()
        {
            Mapper mapper = CreateMapper();
            mockBankRequestRepo = new();

            return new TopUpService(
                mockBankRepo!.Object,
                mockBankRequestRepo.Object,
                mapper
            );
        }

        private static Mapper CreateMapper()
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

        private static void AssertParseableToDateTime(string expiredDate)
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

        [Fact]
        public async void GetBankTopUpRequest_ValidData_ReturnSuccess()
        {
            // Arrange
            Initialize();
            RequestStatus? status = RequestStatus.Pending;
            mockBankRequestRepo.Setup(r => r.GetAll(status)).ReturnsAsync(
                new List<BankTopUpRequest> { new Mock<BankTopUpRequest>().Object });

            // Act & Assert
            List<GetBankTopUpRequestResponseDTO> resDTO = await topUpService.GetBankTopUpRequest(status);

            // Assert
            Assert.Single(resDTO);
            mockBankRequestRepo.Verify(p => p.GetAll(status), Times.Once());
        }
    }
}
