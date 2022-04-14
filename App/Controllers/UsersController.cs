using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Services;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using App.Helpers;
using System.Net;
using System.Reflection;

namespace App.Controllers
{
    [Authorize(Role = "Customer")]
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
            ParsedToken? parsedToken = HttpContext.Items["userAttr"] as ParsedToken;


            uint? currUserId = parsedToken.userId;

            if(currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }

            var response = await _userService.GetProfile(userId);

            return Ok(response);

        }

        [HttpGet("check/{userName}")]
        public async Task<IActionResult> GetDisplayName(string userName)
        {
            var response = await _userService.GetDisplayName(userName);

            return Ok(response);
        }

        [HttpPatch("{userId}")]
        public async Task<IActionResult> UpdateProfile(uint userId, [FromBody] UpdateProfileRequestDTO dto)
        {
            ParsedToken? parsedToken = HttpContext.Items["userAttr"] as ParsedToken;


            uint? currUserId = parsedToken.userId;

            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }

            if(isAllObjectPropertiesNull(dto))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "At least one attribute needs to be updated");
            }

            if((dto.OldPassword == null && dto.NewPassword != null) || (dto.OldPassword != null && dto.NewPassword == null))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Updating password needs two attribute, 'old_password' and 'new_password'");
            }
            

            await _userService.UpdateProfile(userId, dto);
            return Ok(new { message = "Update profile successful" });
        }

        private bool isAllObjectPropertiesNull(Object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if (property.GetValue(obj, null) != null)
                {
                    return false;
                }
            }

            return true;
        }
    }

}
