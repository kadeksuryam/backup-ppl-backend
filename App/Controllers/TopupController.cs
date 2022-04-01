using App.Authorization;
using App.Helpers;
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
    }
}
