namespace App.Services
{
    using App.DTOs.Requests;
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
        public UserService(IUserRepository repository, IMapper mapper)
        {
            _userRepository = repository;
            _mapper = mapper;
        }

        public async Task Register(RegisterRequestDTO dto)
        {
            if (await IsUsernameTaken(dto.Username))
            {
                Console.WriteLine("TEST");
                throw new HttpStatusCodeException(HttpStatusCode.Conflict, "Username '" + dto.Username + "' is already taken");
            }
            Console.WriteLine("TEST");
            var user = _mapper.Map<User>(dto);

            user.EncryptedPassword = BCrypt.HashPassword(dto.Password);

            await _userRepository.Add(user);
        }

        private async Task<bool> IsUsernameTaken(string username)
        {
            return (await _userRepository.Get(username)) != null;
        }
    }

}
