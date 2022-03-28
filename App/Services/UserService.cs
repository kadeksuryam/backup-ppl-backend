namespace App.Services
{
    using App.Authorization;
    using App.DTOs.Requests;
    using App.DTOs.Responses;
    using App.Helpers;
    using App.Models;
    using App.Repositories;
    using AutoMapper;
    using System.Net;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;
        private readonly IBcryptWrapper _bcryptWrapper;
        public UserService(IUserRepository userRepository, ILevelRepository levelRepository,
            IMapper mapper,
            IJwtUtils jwtUtils, IBcryptWrapper bcryptWrapper)
        {
            _userRepository = userRepository;
            _levelRepository = levelRepository;
            _mapper = mapper;
            _jwtUtils = jwtUtils;
            _bcryptWrapper = bcryptWrapper;
        }

        public async Task Register(RegisterRequestDTO dto)
        {
            if (await IsUsernameTaken(dto.Username))
            {
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Username '" + dto.Username + "' is already taken");
            }

            if (await IsEmailTaken(dto.Email))
            {
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Email '" + dto.Email + "' is already taken");
            }

            var user = _mapper.Map<User>(dto);

            user.EncryptedPassword = _bcryptWrapper.hashPassword(dto.Password);

            user.Balance = 0;
            user.Exp = 0;
            user.LevelId = 1;
            user.Type = User.LoginType.Standard;


            await _userRepository.Add(user);
        }

        private async Task<bool> IsUsernameTaken(string username)
        {
            return (await _userRepository.GetByUsername(username)) != null;
        }

        private async Task<bool> IsEmailTaken(string email)
        {
            return (await _userRepository.GetByEmail(email)) != null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO dto)
        {
            User? user = await _userRepository.GetByEmail(dto.Email);

            if (user == null || !_bcryptWrapper.isPasswordCorrect(dto.Password, user.EncryptedPassword))
            {
                throw new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Incorrect email or password");
            }

            var response = _mapper.Map<LoginResponseDTO>(user);
            response.Token = _jwtUtils.GenerateToken(user);
            return response;
        }

        public async Task UpdateProfile(uint userId, UpdateProfileRequestDTO dto)
        {
            User userDb = await _userRepository.GetById(userId);

            if(HasGoogleLoginType(userDb))
            {
                throw GetUpdateProfileGoogleLoginTypeException();
            }

            if(dto.NewDisplayName != null)
            {
                userDb.DisplayName = dto.NewDisplayName;
            }

            if (dto.NewPassword != null && dto.OldPassword != null)
            {
                if(_bcryptWrapper.isPasswordCorrect(dto.OldPassword, userDb!.EncryptedPassword))
                {
                    userDb.EncryptedPassword = _bcryptWrapper.hashPassword(dto.NewPassword);
                }
                else
                {
                    throw GetUpdateProfileIncorrectOldPasswordException();
                }
            }
            await _userRepository.Update(userDb);
        }

        public async Task<GetProfileResponseDTO> GetProfile(uint userId)
        {
            User userDb = await _userRepository.GetById(userId);

            var response = _mapper.Map<GetProfileResponseDTO>(userDb);
            response.Level = (await _levelRepository.GetById(userDb.LevelId)).Name;

            return response;
        }

        public HttpStatusCodeException GetUpdateProfileIncorrectOldPasswordException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Incorrect old password");
        }

        public HttpStatusCodeException GetUpdateProfileGoogleLoginTypeException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Forbidden, "Cannot update with Google account");
        }

        private bool HasGoogleLoginType(User user)
        {
            return user.Type == User.LoginType.Google;
        }
    }

}
