using System.Text.Json.Serialization;
using App.Helpers;

namespace App.DTOs.Responses
{
    [Serializable]
    public class BankTopUpResponseDTO
    {
        [JsonPropertyName("account_number")]
        public long AccountNumber { get; set; }
        [JsonPropertyName("expired_date")]
        [JsonConverter(typeof(DateTimeJSONConverter))]
        public DateTime ExpiredDate { get; set; }
    }
}