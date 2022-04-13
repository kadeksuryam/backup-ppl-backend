using App.Models;
using App.DTOs.Requests;
using App.DTOs.Responses;

namespace App.Services
{
    public interface IUserService
    {
        Task Register(RegisterRequestDTO dto);
        Task<LoginResponseDTO> Login(LoginRequestDTO dto);
        Task UpdateProfile(uint userId, UpdateProfileRequestDTO dto);
        Task<GetProfileResponseDTO> GetProfile(uint userId);
        Task<GetDisplayNameResponseDTO> GetDisplayName(string userName);
    }
}
