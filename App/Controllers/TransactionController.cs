using App.Authorization;
using App.DTOs.Requests;
using App.DTOs.Responses;
using App.Helpers;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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

        [Authorize(Role = "Customer")]
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetTopUpHistoriesByUser(uint userId)
        {
            VerifyUserId(userId);
            List<TransactionHistoryResponseDTO> resDTO = await _transactionService.GetTransactionHistoriesByUser(userId);
            return Ok(resDTO);
        }
        private void VerifyUserId(uint userId)
        {
            uint? currUserId = (uint?)HttpContext.Items["userId"];
            if (currUserId != userId)
            {
                throw new HttpStatusCodeException(HttpStatusCode.Forbidden, "User Id not match!");
            }
        }
    }
}
