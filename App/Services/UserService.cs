namespace App.Services
{
    using App.Authorization;
    using App.DTOs.Requests;
    using App.DTOs.Responses;
    using App.Helpers;
    using App.Models;
    using App.Repositories;
    using AutoMapper;
    using System;
    using System.Net;
    using BCrypt = BCrypt.Net.BCrypt;

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
            var user = _mapper.Map<User>(dto);

            user.EncryptedPassword = _bcryptWrapper.hashPassword(dto.Password);

            await _userRepository.Add(user);
        }

        private async Task<bool> IsUsernameTaken(string username)
        {
            return (await _userRepository.Get(username)) != null;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO dto)
        {
            User user = await _userRepository.GetByEmail(dto.Email);

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
            if (user == null)
            {
                throw GetUpdateProfileUserNotFoundException();
            }
            else if (!_bcryptWrapper.isPasswordCorrect(dto.OldPassword, user.EncryptedPassword))
            {
                throw GetUpdateProfileIncorrectOldPasswordException();
            }
            else if (!IsPasswordFormatValid(dto.NewPassword))
            {
                throw GetUpdateProfileInvalidNewPasswordException();
            }
            else if (!IsDisplayNameValid(dto.NewDisplayName))
            {
                throw GetUpdateProfileInvalidDisplayNameException();
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

        public HttpStatusCodeException GetUpdateProfileUserNotFoundException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Unauthorized);
        }

        public HttpStatusCodeException GetUpdateProfileIncorrectOldPasswordException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Unauthorized, "Incorrect old password");
        }

        public HttpStatusCodeException GetUpdateProfileInvalidNewPasswordException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Forbidden, "Invalid new password");
        }

        public HttpStatusCodeException GetUpdateProfileInvalidDisplayNameException()
        {
            return new HttpStatusCodeException(HttpStatusCode.Forbidden, "Invalid new display name");
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

        private bool IsDisplayNameValid(string displayName)
        {
            // Implement this
            return displayName != "";
        }

        private bool IsPasswordFormatValid(string password)
        {
            // Implement this
            return password != "";
        }
    }

}
