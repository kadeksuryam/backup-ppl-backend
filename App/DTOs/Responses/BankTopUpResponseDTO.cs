using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class BankTopUpResponseDTO
    {
        [JsonPropertyName("account_number")]
        public long AccountNumber { get; set; }
        [JsonPropertyName("expired_date")]
        public string ExpiredDate { get; set; }
    }
}