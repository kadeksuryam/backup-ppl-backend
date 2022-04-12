using App.Authorization;
using App.DTOs.Requests;
using App.Helpers;
using App.Models;
using App.Models.Enums;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;

namespace App.Controllers
{
    [Route("topup")]
    public class TopupController : ControllerBase
    {
        private readonly ITopUpService _topUpService;

        public TopupController(ITopUpService topUpService)
        {
            _topUpService = topUpService;
        }

        [Authorize(Role = "Admin")]
        [HttpGet("requests")]
        public async Task<IActionResult> Get([FromQuery] [BindRequired] string status)
        {
            if(!Enum.TryParse(status, out RequestStatus requestStatus))
            {
                throw new HttpStatusCodeException(HttpStatusCode.BadRequest, "Invalid request status");
            };

            return Ok(new SuccessDetails() {
                StatusCode = (int)HttpStatusCode.OK, 
                Data = await _topUpService.GetBankTopUpRequest(requestStatus)
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
                StatusCode = (int)HttpStatusCode.OK,
                Data = new { message = "Given TopUp Request has been successfully updated" }
            });
        }

        [Authorize(Role = "Admin")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryTopUps([FromQuery] PagingParameters getAllParameters)
        {
            return Ok(new SuccessDetails()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await _topUpService.GetHistoryTransaction(getAllParameters)
            });
        }
    }
}
