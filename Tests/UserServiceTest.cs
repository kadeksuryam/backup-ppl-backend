using App.Data;
using App.DTOs.Requests;
using App.Helpers;
using App.Models;
using App.Repositories;
using App.Services;
using AutoMapper;
using Moq;
using System.Net;
using Xunit;

namespace Tests
{
    public class UserServiceTest
    {
        [Fact]
        public async void RegisterUser_EmailTaken_ReturnsException()
        {
            // Arrange
            string username = "test";
           RegisterRequestDTO dto = new RegisterRequestDTO();
            dto.Username = username;

            User user = new User();
            user.UserName = username;

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(p => p.Get(username)).ReturnsAsync(user);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var userService = new UserService(mockUserRepo.Object, mapper);

            // Act & Assert
            HttpStatusCodeException exception = await Assert.ThrowsAsync<HttpStatusCodeException>(() => userService.Register(dto));

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
            Assert.Equal("Username '" + username + "' is already taken", exception.Message);
            mockUserRepo.Verify(p => p.Get(username), Times.Once());
            mockUserRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Never());
        }

        [Fact]
        public async void RegisterUser_ValidData_ReturnsSuccess()
        {
            // Arrange
            string username = "test";
            string password = "testPassword";
            string email = "test@test.com";
            string name = "name";

            RegisterRequestDTO dto = new RegisterRequestDTO();
            dto.Username = username;
            dto.Password = password;
            dto.Email = email;
            dto.Name = name;

            var mockUserRepo = new Mock<IUserRepository>();
            mockUserRepo.Setup(p => p.Get(username)).ReturnsAsync((User)null);
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = mockMapper.CreateMapper();
            var user = mapper.Map<User>(dto);

            var userService = new UserService(mockUserRepo.Object, mapper);

            // Act
             userService.Register(dto);

            // Assert
            mockUserRepo.Verify(p => p.Get(username), Times.Once());
            mockUserRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Once());
        }
    }
}
