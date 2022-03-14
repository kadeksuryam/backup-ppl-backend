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
        private readonly IMapper _mapper;
        private readonly IJwtUtils _jwtUtils;
        private readonly IBcryptWrapper _bcryptWrapper;
        public UserService(IUserRepository repository, IMapper mapper,
            IJwtUtils jwtUtils, IBcryptWrapper bcryptWrapper)
        {
            _userRepository = repository;
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

        public async Task UpdateProfile(UpdateProfileRequestDTO dto)
        {
            User user = await _userRepository.Get(dto.UserName);
            if (!_bcryptWrapper.isPasswordCorrect(dto.OldPassword, user.EncryptedPassword))
            {
                throw GetUpdateProfileIncorrectOldPasswordException();
            }
            else if (HasGoogleLoginType(user))
            {
                throw GetUpdateProfileGoogleLoginTypeException();
            }
            else
            {
                user.DisplayName = dto.NewDisplayName;
                user.EncryptedPassword = _bcryptWrapper.hashPassword(dto.NewPassword);
                await _userRepository.Update(user);
            }
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
            // Implement it. For now, assume all user are standard type.
            return false;
        }
    }

}
