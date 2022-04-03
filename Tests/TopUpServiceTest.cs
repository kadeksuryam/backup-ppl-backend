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
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class TopUpServiceTest
    {
        private Mock<IBankRepository>? mockBankRepo;
        private Bank? mockBank;
        private Mock<IBankTopUpRequestRepository>? mockBankRequestRepo;
        private Mock<IVoucherRepository>? mockVoucherRepo;
        private Mock<IUserRepository>? mockUserRepo;
        private Mock<ITopUpHistoryRepository>? mockHistoryRepo;
        private MapperConfiguration? mapperConfig;
        private Mapper? mapper;
        private TopUpService? topUpService;
        private VoucherService? voucherService;
        private Voucher? mockVoucher;
        private User? mockUser;

        private void Initialize()
        {
            mockBankRequestRepo = new Mock<IBankTopUpRequestRepository>();
            mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            mapper = new Mapper(mapperConfig);
            mockBankRepo = new();
            mockVoucherRepo = new();
            mockUserRepo = new();
            mockHistoryRepo = new();

            voucherService = new VoucherService(mockVoucherRepo.Object, mapper);
            topUpService = new TopUpService(mockBankRepo.Object,
                mockBankRequestRepo.Object, mockVoucherRepo.Object,
                mockUserRepo.Object, mockHistoryRepo.Object,
                voucherService, mapper);
        }
        private uint GetAuthenticatedUserId()
        {
            uint userId = 1234;
            MakeUserIdAuthenticated(userId);
            return userId;
        }

        private void MakeUserIdAuthenticated(uint userId)
        {
            mockUser = new();
            mockUser.Id = userId;
            mockUser.Balance = 0;
            mockUserRepo!.Setup(repo => repo.GetById(userId)).ReturnsAsync(mockUser);
        }

        [Fact]
        public async void BankTopUp_ValidRequest_ReturnsSuccessAsync()
        {
            Initialize();
            uint userId = GetAuthenticatedUserId();
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

            MakeBankRequestValid(request);
            return request;
        }

        private void MakeBankRequestValid(BankTopUpRequestDTO request)
        {
            mockBank = new();
            mockBank.Id = request.BankId;
            mockBankRepo!.Setup(repo => repo.GetById(request.BankId)).ReturnsAsync(mockBank);
        }

        private async Task<BankTopUpResponseDTO> MakeBankTopUp(uint userId, BankTopUpRequestDTO request)
        {
            return await topUpService!.BankTopUp(userId, request);
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
            mockBankRequestRepo!.Setup(r => r.GetAll(status)).ReturnsAsync(
                new List<BankTopUpRequest> { new Mock<BankTopUpRequest>().Object });

            // Act & Assert
            List<GetBankTopUpRequestResponseDTO> resDTO = await topUpService!.GetBankTopUpRequest(status);

            // Assert
            Assert.Single(resDTO);
            mockBankRequestRepo.Verify(p => p.GetAll(status), Times.Once());
        }

        [Fact]
        public async void VoucherTopUp_ValidData_ReturnsSuccess()
        {
            Initialize();
            uint userId = GetAuthenticatedUserId();
            VoucherTopUpRequestDTO request = GetValidVoucherTopUpRequest();
            VoucherTopUpResponseDTO response = await MakeVoucherTopUp(userId, request);

            AssertValidVoucherTopUpResponse(response);
            AssertExactlyOneVoucherUpdate(request);
            AssertExactlyOneUserUpdate();
            AssertExactlyOneTopUpHistoryAdded();
        }

        private VoucherTopUpRequestDTO GetValidVoucherTopUpRequest()
        {
            VoucherTopUpRequestDTO request = new();
            request.VoucherCode = "FREEMONEY";

            MakeVoucherRequestValid(request);
            return request;
        }

        private void MakeVoucherRequestValid(VoucherTopUpRequestDTO request)
        {
            mockVoucher = new();
            mockVoucher.Code = request.VoucherCode;
            mockVoucher.Amount = 25000;
            mockVoucher.IsUsed = false;

            mockVoucherRepo!.Setup(repo => repo.GetByCode(request.VoucherCode)).ReturnsAsync(mockVoucher);
        }

        private async Task<VoucherTopUpResponseDTO> MakeVoucherTopUp(uint userId, VoucherTopUpRequestDTO request)
        {
            return await topUpService!.VoucherTopUp(userId, request);
        }

        private void AssertValidVoucherTopUpResponse(VoucherTopUpResponseDTO response)
        {
            Assert.Equal(mockVoucher!.Amount, response.Amount);
        }

        private void AssertExactlyOneVoucherUpdate(VoucherTopUpRequestDTO request)
        {
            mockVoucherRepo!.Verify(
                repo => repo.Update(It.IsAny<Voucher>()),
                Times.Once
            );
        }

        private void AssertExactlyOneUserUpdate()
        {
            mockUserRepo!.Verify(
                repo => repo.Update(It.IsAny<User>()),
                Times.Once
            );
        }

        private void AssertExactlyOneTopUpHistoryAdded()
        {
            mockHistoryRepo!.Verify(
                repo => repo.Add(It.IsAny<TopUpHistory>()),
                Times.Once
            );
        }
    }
}
