using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
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
        public IActionResult TestAuth()
        {
            return Ok(new { message = "success" });
        }

    }

}
