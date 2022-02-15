using App.DTOs.Requests;

namespace App.Services
{
    public interface IUserService
    {
        Task Register(RegisterRequestDTO dto);
    }
}
