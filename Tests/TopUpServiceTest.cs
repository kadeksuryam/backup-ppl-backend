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
        private Mock<IUserRepository>? mockUserRepo;
        private Mock<ITopUpHistoryRepository>? mockHistoryRepo;
        private Mock<IVoucherService>? mockVoucherService;
        private MapperConfiguration? mapperConfig;
        private Mapper? mapper;
        private TopUpService? topUpService;
        private Voucher? mockVoucher;
        private User? mockUser;
        private List<TopUpHistory> mockHistories = new();

        private void Initialize()
        {
            mockBankRequestRepo = new Mock<IBankTopUpRequestRepository>();
            mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            mapper = new Mapper(mapperConfig);
            mockBankRepo = new();
            mockUserRepo = new();
            mockHistoryRepo = new();
            mockVoucherService = new Mock<IVoucherService>();

            topUpService = new TopUpService(mockBankRepo.Object,
                mockBankRequestRepo.Object,
                mockUserRepo.Object, mockHistoryRepo.Object,
                mockVoucherService.Object, mapper);
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
            AssertExactlyOneVoucherUsage();
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

            mockVoucherService!.Setup(service => service.UseVoucher(mockVoucher.Code))
                .ReturnsAsync(MarkAsUsedAndGetMockVoucher);
        }
        private Voucher MarkAsUsedAndGetMockVoucher()
        {
            mockVoucher!.IsUsed = true;
            return mockVoucher;
        }

        private async Task<VoucherTopUpResponseDTO> MakeVoucherTopUp(uint userId, VoucherTopUpRequestDTO request)
        {
            return await topUpService!.VoucherTopUp(userId, request);
        }

        private void AssertValidVoucherTopUpResponse(VoucherTopUpResponseDTO response)
        {
            Assert.Equal(mockVoucher!.Amount, response.Amount);
        }

        private void AssertExactlyOneVoucherUsage()
        {
            mockVoucherService!.Verify(
                repo => repo.UseVoucher(It.IsAny<string>()),
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

        [Fact]
        public async void GetTopUpHistoriesByUser_ValidData_ReturnSuccess()
        {
            Initialize();
            uint userId = GetAuthenticatedUserId();
            MakeMockTopUpHistoriesByUserId(userId);
            List<TopUpHistoryResponseDTO> response = await topUpService!.GetTopUpHistoriesByUser(userId);
            AssertTopUpHistoriesResponseSortedByTime(response);
        }

        private void MakeMockTopUpHistoriesByUserId(uint userId)
        {
            mockHistories = new();
            mockHistories.Add(new TopUpHistory() {
                UpdatedAt = DateTime.Now,
                Method = TopUpHistory.TopUpMethod.Bank
            });
            mockHistories.Add(new TopUpHistory()
            {
                UpdatedAt = DateTime.Now.AddHours(2),
                Method = TopUpHistory.TopUpMethod.Voucher
            });
            mockHistories.Add(new TopUpHistory()
            {
                UpdatedAt = DateTime.Now.AddHours(1),
                Method = TopUpHistory.TopUpMethod.Voucher
            });

            mockHistoryRepo!.Setup(repo => repo.GetAllByUserId(userId)).ReturnsAsync(mockHistories);
        }

        private void AssertTopUpHistoriesResponseSortedByTime(List<TopUpHistoryResponseDTO> response)
        {
            // Later first
            Assert.True(DateTime.TryParse(
                response[0].UpdatedAt,
                out DateTime firstUpdateTime
            ));

            int responseIndex = 1;
            while (responseIndex < mockHistories.Count)
            {
                Assert.True(DateTime.TryParse(
                    response[responseIndex].UpdatedAt,
                    out DateTime secondUpdateTime
                ));
                Assert.True(DateTime.Compare(firstUpdateTime, secondUpdateTime) > 0);

                firstUpdateTime = secondUpdateTime;
                responseIndex++;
            }
        }
    }
}
