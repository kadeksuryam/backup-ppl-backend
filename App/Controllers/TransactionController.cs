using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Services;
using App.Repositories;
using Microsoft.AspNetCore.Mvc;
using App.Helpers;
using System.Net;
using System.Reflection;
using App.Models;
using System.Text.Json;
using System.ComponentModel;

namespace App.Controllers
{
    [ApiController]
    [Route("transactions")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        [Authorize(Role = "Customer")]
        [HttpPost()]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequestDTO dto)
        {
            ParsedToken? parsedToken = HttpContext.Items["userAttr"] as ParsedToken;

            uint? currUserId = parsedToken.userId;

            if (currUserId != dto.FromUserId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }

            var response = await _transactionService.CreateTransaction(dto);
            return Ok(response);
        }

        [Authorize(Role = "Admin")]
        [HttpGet("history")]
        public async Task<IActionResult> GetHistoryTransactions([FromQuery] PagingParameters getAllParameters) {
            return Ok(new SuccessDetails()
            {
                StatusCode = (int)HttpStatusCode.OK,
                Data = await _transactionService.GetHistoryTransaction(getAllParameters)
            });
        }
    }
}
