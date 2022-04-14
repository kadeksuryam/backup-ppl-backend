using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Models;
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
                Data = await _topUpService.GetBankTopUpRequest(requestStatus)
            });
        }

        private void VerifyUserId(uint userId)
        {
            ParsedToken? parsedToken = HttpContext.Items["userAttr"] as ParsedToken;


            uint? currUserId = parsedToken!.userId;
            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }
        }

        [Authorize(Role = "Customer")]
        [HttpPost("bank")]
        public async Task<IActionResult> BankTopUp([FromBody] BankTopUpRequestDTO reqDTO)
        {
            VerifyUserId(reqDTO.UserId);
            BankTopUpResponseDTO resDTO = await _topUpService.BankTopUp(reqDTO);

            return Ok(new SuccessDetails()
            {
                Data = resDTO
            });
        }

        [Authorize(Role = "Customer")]
        [HttpPost("voucher")]
        public async Task<IActionResult> VoucherTopUp([FromBody] VoucherTopUpRequestDTO reqDTO)
        {
            VerifyUserId(reqDTO.UserId);
            VoucherTopUpResponseDTO resDTO = await _topUpService.VoucherTopUp(reqDTO);
            return Ok(new SuccessDetails()
            {
                Data = resDTO
            });
        }

        [Authorize(Role = "Customer")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetTopUpHistoriesByUser(uint userId)
        {
            VerifyUserId(userId);
            List<TopUpHistoryResponseDTO> resDTO = await _topUpService.GetTopUpHistoriesByUser(userId);
            return Ok(new SuccessDetails()
            {
                Data = resDTO
            });
        }

        [Authorize(Role = "Admin")]
        [HttpPatch("requests")]
        public async Task<IActionResult> UpdateRequestTopUp([FromBody] UpdateTopUpRequestStatusRequestDTO dto)
        {
            if (!Enum.TryParse(dto.Status, out RequestStatus requestStatus))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Invalid request status");
            };

            await _topUpService.UpdateBankTopUpRequest(dto);

            return Ok(new SuccessDetails()
            {
                Data = new { message = "Given TopUp Request has been successfully updated" }
            });
        }

        [Authorize(Role = "Admin")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryTopUps([FromQuery] PagingParameters getAllParameters)
        {
            return Ok(new SuccessDetails()
            {
                Data = await _topUpService.GetHistoryTransaction(getAllParameters)
            });

        }
    }
}
