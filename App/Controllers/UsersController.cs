using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Services;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using App.Helpers;
using System.Net;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService, ILevelRepository levelRepository)
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

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetProfile(uint userId)
        {
            uint? currUserId = (uint)HttpContext.Items["userId"];

            if(currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }

            var response = await _userService.GetProfile(userId);

            return Ok(response);

        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateProfile(uint userId, [FromBody] UpdateProfileRequestDTO dto)
        {
            uint? currUserId = (uint)HttpContext.Items["userId"];

            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }

            await _userService.UpdateProfile(userId, dto);
            return Ok(new { message = "Update profile successful" });
        }

    }

}
