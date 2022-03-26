using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Services;
using App.Repositories;
using App.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILevelRepository _levelRepository;

        public UsersController(IUserService userService, ILevelRepository levelRepository)
        {
            _userService = userService;
            _levelRepository = levelRepository;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO dto)
        {
            await _userService.Register(dto);
            return Ok(new { message = "Registration successful" });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO reqDTO)
        {
            LoginResponseDTO resDTO = await _userService.Login(reqDTO);
            return Ok(resDTO);
        }

        [HttpGet]
        public async Task<IActionResult> ViewProfile()
        {
            User? user = HttpContext.Items["User"] as User;

            if (user == null)
            {
                return NotFound();
            }

            ViewProfileResponseDTO resDTO = new ViewProfileResponseDTO();
            resDTO.Id = user.Id;
            resDTO.Email = user.Email;
            resDTO.UserName = user.UserName;
            resDTO.DisplayName = user.DisplayName;
            resDTO.EXP = user.Exp;
            resDTO.Balance = user.Balance;

            Level? level = await _levelRepository.GetById(user.LevelId);
            resDTO.Level = level.Name;


            return Ok(resDTO);
        }

        [HttpPut("{dto.UserId}")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequestDTO dto)
        {
            await _userService.UpdateProfile(dto);
            return Ok(new { message = "Update profile successful" });
        }

    }

}
