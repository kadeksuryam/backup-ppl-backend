using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
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
        public async void RegisterUser_UsernameTaken_ReturnsException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            string username = "test";
            RegisterRequestDTO dto = new RegisterRequestDTO();
            dto.Username = username;

            User user = new User();
            user.UserName = username;
            mockUserRepo.Setup(p => p.GetByUsername(username)).ReturnsAsync((User)user);

            var userService = new UserService(mockUserRepo.Object, mapper,
                mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act & Assert
            HttpStatusCodeException exception = await Assert.ThrowsAsync<HttpStatusCodeException>(() => userService.Register(dto));

            // Assert
            Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
            Assert.Equal("Username '" + username + "' is already taken", exception.Message);
            mockUserRepo.Verify(p => p.GetByUsername(username), Times.Once());
            mockUserRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Never());
        }

        [Fact]
        public async void RegisterUser_ValidData_ReturnsSuccess()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            string username = "test";
            string password = "testPassword";
            string email = "test@test.com";
            string name = "name";

            RegisterRequestDTO dto = new RegisterRequestDTO();
            dto.Username = username;
            dto.Password = password;
            dto.Email = email;
            dto.Name = name;

            mockUserRepo.Setup(p => p.GetByUsername(username)).ReturnsAsync((User)null);

            var userService = new UserService(mockUserRepo.Object, mapper,
               mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act
            await userService.Register(dto);

            // Assert
            mockUserRepo.Verify(p => p.GetByUsername(username), Times.Once());
            mockUserRepo.Verify(r => r.Add(It.IsAny<User>()), Times.Once());
        }

        [Fact]
        public async void LoginUser_UserNotFound_ReturnsException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            LoginRequestDTO dto = new LoginRequestDTO();
            dto.Email = "test@test.com";
            dto.Password = "test";

            mockUserRepo.Setup(p => p.GetByEmail(dto.Email)).ReturnsAsync((User)null);
            var userService = new UserService(mockUserRepo.Object, mapper,
                 mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act & Assert
            HttpStatusCodeException exception = await Assert.ThrowsAsync<HttpStatusCodeException>(() => userService.Login(dto));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Equal("Incorrect email or password", exception.Message);
            mockUserRepo.Verify(p => p.GetByEmail(dto.Email), Times.Once());
        }

        [Fact]
        public async void LoginUser_IncorrectPassword_ReturnsException()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            LoginRequestDTO dto = new LoginRequestDTO();
            dto.Email = "test@test.com";
            dto.Password = "test";
            User user = new User();
            user.EncryptedPassword = "testEncrypt";

            mockUserRepo.Setup(p => p.GetByEmail(dto.Email)).ReturnsAsync((User)user);
            mockBcryptWrapper.Setup(p => p.isPasswordCorrect(dto.Password, user.EncryptedPassword)).Returns(false);

            var userService = new UserService(mockUserRepo.Object, mapper,
                mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act & Assert
            HttpStatusCodeException exception = await Assert.ThrowsAsync<HttpStatusCodeException>(() => userService.Login(dto));

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, exception.StatusCode);
            Assert.Equal("Incorrect email or password", exception.Message);
            mockUserRepo.Verify(p => p.GetByEmail(dto.Email), Times.Once());

        }

        [Fact]
        public async void LoginUser_ValidData_ReturnsSuccess()
        {
            // Arrange
            var mockUserRepo = new Mock<IUserRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            LoginRequestDTO dto = new LoginRequestDTO();
            dto.Email = "test@test.com";
            dto.Password = "test";
            User user = new User();
            user.EncryptedPassword = "testEncrypt";
            string token = "TOKEN_TEST";

            mockUserRepo.Setup(p => p.GetByEmail(dto.Email)).ReturnsAsync((User)user);
            mockBcryptWrapper.Setup(p => p.isPasswordCorrect(dto.Password, user.EncryptedPassword)).Returns(true);
            mockJwtUtil.Setup(p => p.GenerateToken(user)).Returns(token);

            var userService = new UserService(mockUserRepo.Object, mapper,
                mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act
            LoginResponseDTO resDTO = await userService.Login(dto);

            // Assert
            Assert.NotNull(resDTO.Token);
            mockUserRepo.Verify(p => p.GetByEmail(dto.Email), Times.Once());
            mockBcryptWrapper.Verify(p => p.isPasswordCorrect(dto.Password, user.EncryptedPassword), Times.Once());
            mockJwtUtil.Verify(p => p.GenerateToken(user), Times.Once());
        }
    }
}
