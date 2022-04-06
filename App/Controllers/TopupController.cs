using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models.Enums;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace App.Controllers
{
    [ApiController]
    [Route("topup")]
    public class TopUpController : ControllerBase
    {
        private readonly ITopUpService _topUpService;

        public TopUpController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [Authorize(Role = "Admin")]
        [HttpGet("requests")]
        public async Task<IActionResult> Get([FromQuery][BindRequired] string status)
        {
            if (!Enum.TryParse(status, out RequestStatus requestStatus))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Invalid request status");
            };

            return Ok(new SuccessDetails()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await _topUpService.GetBankTopUpRequest(requestStatus)
            });
        }

        [Authorize(Role = "Customer")]
        [HttpPost("users/{userId}/bank/make-request")]
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

        [Authorize(Role = "Customer")]
        [HttpPost("users/{userId}/voucher/use")]
        public async Task<IActionResult> VoucherTopUp(uint userId, [FromBody] VoucherTopUpRequestDTO reqDTO)
        {
            uint? currUserId = (uint?)HttpContext.Items["userId"];

            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }
            else
            {
                VoucherTopUpResponseDTO resDTO = await _topUpService.VoucherTopUp(userId, reqDTO);
                return Ok(resDTO);
            }
        }
    }
}
