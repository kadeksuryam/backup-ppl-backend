using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Models.Enums;
using App.Repositories;
using App.Services;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace Tests
{
    public class TopUpServiceTest
    {
        Mock<IBankTopUpRequestRepository>? mockTopUpReqRepo;
        MapperConfiguration? mapperConfig;
        Mapper? mapper;
        TopUpService? topUpService;

        private void Initialize()
        {
            mockTopUpReqRepo = new Mock<IBankTopUpRequestRepository>();
            mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            mapper = new Mapper(mapperConfig);
            topUpService = new TopUpService(mockTopUpReqRepo.Object, mapper);
        }

        [Fact]
        public async void GetBankTopUpRequest_ValidData_ReturnSuccess()
        {
            // Arrange
            Initialize();
            RequestStatus? status = RequestStatus.Pending;
            mockTopUpReqRepo.Setup(r => r.GetAll(status)).ReturnsAsync(
                new List<BankTopUpRequest> { new Mock<BankTopUpRequest>().Object });

            // Act & Assert
            List<GetBankTopUpRequestResponseDTO> resDTO = await topUpService.GetBankTopUpRequest(status);

            // Assert
            Assert.Single(resDTO);
            mockTopUpReqRepo.Verify(p => p.GetAll(status), Times.Once());
        }
    }
}
