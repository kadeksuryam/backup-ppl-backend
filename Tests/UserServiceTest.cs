using App.Authorization;
using App.Data;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
using App.Repositories;
using App.Services;
using AutoMapper;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Tests
{
    public class UserServiceTest
    {

        [Fact]
        public async void RegisterUser_UsernameTaken_ReturnsException()
        {
            // Arrange
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();

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

            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object, mockLevelRepo.Object, mapper,
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
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();
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

            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object, mockLevelRepo.Object, mapper,
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
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();
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
            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object, mockLevelRepo.Object, mapper,
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
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();
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

            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object,  mockLevelRepo.Object, mapper,
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
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();
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

            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object, mockLevelRepo.Object, mapper,
                mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act
            LoginResponseDTO resDTO = await userService.Login(dto);

            // Assert
            Assert.NotNull(resDTO.Token);
            mockUserRepo.Verify(p => p.GetByEmail(dto.Email), Times.Once());
            mockBcryptWrapper.Verify(p => p.isPasswordCorrect(dto.Password, user.EncryptedPassword), Times.Once());
            mockJwtUtil.Verify(p => p.GenerateToken(user), Times.Once());
        }

        [Fact]
        public async void GetProfile_ValidData_ReturnsSuccess()
        {
            // Arrange
            var mockDataContext = new Mock<IDataContext>();
            var mockUserRepo = new Mock<IUserRepository>();
            var mockLevelRepo = new Mock<ILevelRepository>();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            var mapper = new Mapper(mapperConfig);
            var mockJwtUtil = new Mock<IJwtUtils>();
            var mockBcryptWrapper = new Mock<IBcryptWrapper>();

            User user = new User()
            {
                Id = 1,
                UserName = "test_username",
                Email = "test_email@email.com",
                DisplayName = "Test Name",
                Balance = 10000,
                Exp = 10,
                LevelId = 1,
                EncryptedPassword = "testEncrypt",
                Type = User.LoginType.Standard,
                Role = User.UserRole.Customer
            };
            Level level = new Level()
            {
                Id = 1,
                Name = "Bronze",
                RequiredExp = 0
            };

            mockUserRepo.Setup(p => p.GetById(user.Id)).ReturnsAsync((User)user);
            mockLevelRepo.Setup(p => p.GetById(user.LevelId)).ReturnsAsync((Level)level);

            var userService = new UserService(mockDataContext.Object, mockUserRepo.Object, mockLevelRepo.Object, mapper,
                mockJwtUtil.Object, mockBcryptWrapper.Object);

            // Act
            GetProfileResponseDTO resDTO = await userService.GetProfile(1);

            // Assert
            mockUserRepo.Verify(p => p.GetById(user.Id), Times.Once());
            Assert.Equal(user.Id.ToString(), resDTO.Id.ToString());
            Assert.Equal(user.UserName.ToString(), resDTO.UserName.ToString());
            Assert.Equal(user.Email.ToString(), resDTO.Email.ToString());
            Assert.Equal("Bronze", resDTO.Level.ToString());
        }

        /* For update profile */
        private Mock<IDataContext> _mockDataContext;
        private Mock<IUserRepository>? _mockUserRepoForUpdateProfileTest;
        private Mock<ILevelRepository>? _mockLevelRepo;
        private Mapper? _mapperForUpdateProfileTest;
        private Mock<IJwtUtils>? _mockJwtUtil;
        private Mock<IBcryptWrapper>? _mockBcryptWrapper;

        private UpdateProfileRequestDTO? _updateProfileRequestDTO;
        private uint _userIdForUpdateProfileTest;
        private User? _userForUpdateProfileTest;
        private UserService? _userServiceForUpdateProfileTest;
        private bool _isOldPasswordMatchesWithItsEncrypt;
        private bool _hasGoogleAccount;

        private HttpStatusCodeException? _actualHttpExceptionFromUpdateProfileTest;
        private HttpStatusCodeException? _expectedHttpExceptionFromUpdateProfileTest;

        private const int UserIdForUpdateProfileTest = 1234;
        private const string OldPasswordForUpdateProfileTest = "testOldPassword";
        private const string NewPasswordForUpdateProfileTest = "testNewPassword";
        private const string OldDisplayNameForUpdateProfileTest = "testOldDisplayName";
        private const string NewDisplayNameForUpdateProfileTest = "testNewDisplayName";
        private const string OldEncryptedPasswordForUpdateProfileTest = "testOldEncryptedPassword";
        private const string NewEncryptedPasswordForUpdateProfileTest = "testNewEncryptedPassword";
        private const string TokenForUpdateProfileTest = "testToken";

        [Fact]
        public async void UpdateProfileUser_ValidData_ReturnsSuccess()
        {
            // Arrange
            _isOldPasswordMatchesWithItsEncrypt = true;
            _hasGoogleAccount = false;
            InitializeUpdateProfileTest();

            // Act
            await UpdateProfileForUpdateProfileTest();

            // Assert
            AssertUpdateProfileRequestFulfilled();
        }

        private void AssertUpdateProfileRequestFulfilled()
        {
            Assert.Equal(_updateProfileRequestDTO!.NewDisplayName, _userForUpdateProfileTest!.DisplayName);
            Assert.Equal(NewEncryptedPasswordForUpdateProfileTest, _userForUpdateProfileTest.EncryptedPassword);
        }

        [Fact]
        public async void UpdateProfileUser_InvalidCorrectPassword_ReturnsException()
        {
            // Arrange
            _isOldPasswordMatchesWithItsEncrypt = false;
            _hasGoogleAccount = false;
            InitializeUpdateProfileTest();

            // Act & Assert
            await AssertUpdateProfileRequestThrowsHttpException();

            // Assert
            AssertHttpExceptionComesFromUpdateProfileIncorrectOldPasswordHandler();
        }

        [Fact]
        public async void UpdateProfileUser_GoogleAccount_ReturnsException()
        {
            // Arrange
            _isOldPasswordMatchesWithItsEncrypt = true;
            _hasGoogleAccount = true;
            InitializeUpdateProfileTest();

            // Act & Assert
            await AssertUpdateProfileRequestThrowsHttpException();

            // Assert
            AssertHttpExceptionComesFromGoogleLoginTypeException();
        }

        private async Task AssertUpdateProfileRequestThrowsHttpException()
        {
            _actualHttpExceptionFromUpdateProfileTest = await Assert.ThrowsAsync<HttpStatusCodeException>(
                () => UpdateProfileForUpdateProfileTest()
            );
        }

        private async Task UpdateProfileForUpdateProfileTest()
        {
            await _userServiceForUpdateProfileTest!.UpdateProfile(_userIdForUpdateProfileTest, _updateProfileRequestDTO!);
        }

        private void AssertHttpExceptionComesFromUpdateProfileIncorrectOldPasswordHandler()
        {
            _expectedHttpExceptionFromUpdateProfileTest = _userServiceForUpdateProfileTest!.GetUpdateProfileIncorrectOldPasswordException();
            AssertSameHttpException();
        }

        private void AssertHttpExceptionComesFromGoogleLoginTypeException()
        {
            _expectedHttpExceptionFromUpdateProfileTest = _userServiceForUpdateProfileTest!.GetUpdateProfileGoogleLoginTypeException();
            AssertSameHttpException();
        }

        private void AssertSameHttpException()
        {
            Assert.Equal(_expectedHttpExceptionFromUpdateProfileTest!.StatusCode,_actualHttpExceptionFromUpdateProfileTest!.StatusCode);
            Assert.Equal(_expectedHttpExceptionFromUpdateProfileTest!.Message, _actualHttpExceptionFromUpdateProfileTest!.Message);
        }

        private void InitializeUpdateProfileTest()
        {
            InitializeUserForUpdateProfileTest();
            InitializeMockAndMapperForUpdateProfileTest();
            InitializeRequestDTOForUpdateProfileTest();
            SetupMockForUpdateProfileTest();
            InitializeUserServiceForUpdateProfileTest();
        }

        private void InitializeMockAndMapperForUpdateProfileTest() {
            _mockDataContext = new Mock<IDataContext>();
            _mockUserRepoForUpdateProfileTest = new Mock<IUserRepository>();
            _mapperForUpdateProfileTest = new Mapper(
                new MapperConfiguration(cfg => {
                    cfg.AddProfile(new AutoMapperProfile());
                })
            );
            _mockJwtUtil = new Mock<IJwtUtils>();
            _mockBcryptWrapper = new Mock<IBcryptWrapper>();
            _mockLevelRepo = new Mock<ILevelRepository>();
        }

        private void InitializeUserForUpdateProfileTest()
        {
            _userForUpdateProfileTest = new User
            {
                Id = UserIdForUpdateProfileTest,
                EncryptedPassword = OldEncryptedPasswordForUpdateProfileTest,
                DisplayName = OldDisplayNameForUpdateProfileTest,
                Type = _hasGoogleAccount ? User.LoginType.Google : User.LoginType.Standard
            };
        }

        private void InitializeRequestDTOForUpdateProfileTest()
        {
            _updateProfileRequestDTO = new UpdateProfileRequestDTO
            {
                OldPassword = OldPasswordForUpdateProfileTest,
                NewPassword = NewPasswordForUpdateProfileTest,
                NewDisplayName = NewDisplayNameForUpdateProfileTest
            };
        }

        private void SetupMockForUpdateProfileTest()
        {
            //_mockUserRepoForUpdateProfileTest!.Setup(p => p.GetById(UserIdForUpdateProfileTest)).ReturnsAsync(_userForUpdateProfileTest!);
            _mockUserRepoForUpdateProfileTest!.Setup(p => p.GetById(_userIdForUpdateProfileTest)).ReturnsAsync(_userForUpdateProfileTest!);
            _mockBcryptWrapper!.Setup(p => p.isPasswordCorrect(OldPasswordForUpdateProfileTest, OldEncryptedPasswordForUpdateProfileTest)).Returns(_isOldPasswordMatchesWithItsEncrypt);
            _mockBcryptWrapper!.Setup(p => p.hashPassword(NewPasswordForUpdateProfileTest)).Returns(NewEncryptedPasswordForUpdateProfileTest);
            _mockJwtUtil!.Setup(p => p.GenerateToken(_userForUpdateProfileTest!)).Returns(TokenForUpdateProfileTest);
            /*            _mockUserRepo!.Setup(p => p.GetById(_userIdForUpdateProfileTest)).ReturnsAsync(
                            new User
                            {
                                Id = UserIdForUpdateProfileTest,
                                EncryptedPassword = OldEncryptedPasswordForUpdateProfileTest,
                                DisplayName = OldDisplayNameForUpdateProfileTest,
                                Type = _hasGoogleAccount ? User.LoginType.Google : User.LoginType.Standard
                            }
                         )


                            .ReturnsAsync(new User
                        {
                            Id = UserIdForUpdateProfileTest,
                            EncryptedPassword = OldEncryptedPasswordForUpdateProfileTest,
                            DisplayName = OldDisplayNameForUpdateProfileTest,
                            Type = _hasGoogleAccount ? User.LoginType.Google : User.LoginType.Standard
                        }));
                    }*/
        }

        private void InitializeUserServiceForUpdateProfileTest()
        {
            _userServiceForUpdateProfileTest = new UserService(_mockDataContext.Object, _mockUserRepoForUpdateProfileTest!.Object, _mockLevelRepo!.Object, _mapperForUpdateProfileTest!,
                _mockJwtUtil!.Object, _mockBcryptWrapper!.Object);
        }
    }
}
