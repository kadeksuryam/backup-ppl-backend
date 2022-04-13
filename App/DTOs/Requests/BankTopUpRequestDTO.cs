using App.Helpers.FormatValidator;
using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
    public class BankTopUpRequestDTO
    {
        [JsonPropertyName("user_id")]
        public uint UserId { get; set; }

        [JsonPropertyName("amount")]
        [Amount]
        public int Amount { get; set; }

        [JsonPropertyName("bank_id")]
        public uint BankId { get; set; }
    }
}