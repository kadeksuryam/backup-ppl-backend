using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Controllers
{
    [Authorize]
    [ApiController]
    [Route("users/{userId}/topups")]
    public class TopUpsController : ControllerBase
    {
        private readonly ITopUpService _topUpService;
        public TopUpsController(ITopUpService bankTopUpService)
        {
            _topUpService = bankTopUpService;
        }

        [HttpPost("bank")]
        public async Task<IActionResult> BankTopUp(uint userId, [FromBody] BankTopUpRequestDTO reqDTO)
        {
            uint? currUserId = (uint?)HttpContext.Items["userId"];

            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }
            else
            {
                BankTopUpResponseDTO resDTO = await _topUpService.BankTopUp(userId, reqDTO);
                return Ok(resDTO);
            }
        }
    }
}
