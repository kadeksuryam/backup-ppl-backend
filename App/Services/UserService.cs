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
    }

}
