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
    }
}
