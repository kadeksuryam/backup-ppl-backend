using App.Authorization;
using App.Helpers;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace App.Controllers
{
    [Authorize(Role = "Customer")]
    [ApiController]
    [Route("voucher")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucherService _voucherService;

        public VoucherController(IVoucherService voucherService)
        {
            _voucherService = voucherService;
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetByCode(string code)
        {
            var voucher = await _voucherService.GetByCode(code);

            return Ok(new SuccessDetails()
            {
                Data = voucher
            });
        }
    }
}
