using System;
using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Services;
using Microsoft.AspNetCore.Mvc;


namespace App.Controllers
{
    [ApiController]
    [Route("banks")]
    public class BankController : ControllerBase
	{
		private readonly IBankService _bankService;

		public BankController(IBankService bankService)
		{
			_bankService = bankService;
		}

        [Authorize(Role = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddNewBank([FromBody] AddBankRequestDTO dto)
        {
            await _bankService.AddNewBank(dto);
            return Ok(new SuccessDetails()
            {
                Data = new { message = "Add new bank successful" }
            });
        }

        [Authorize(Role = "Customer")]
        [HttpGet]
        public async Task<IActionResult> GetAllBank()
        {
            GetAllBankResponseDTO resDTO = await _bankService.GetAllBank();
            return Ok(new SuccessDetails()
            {
                Data = resDTO
            });
        }
    }
}

