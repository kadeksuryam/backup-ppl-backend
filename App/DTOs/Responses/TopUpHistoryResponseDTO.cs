using App.Helpers;
using App.Helpers.FormatValidator;
using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class TopUpHistoryResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(DateTimeJSONConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        [JsonConverter(typeof(DateTimeJSONConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; } = "";

        [JsonPropertyName("bank_request_id")]
        public uint BankRequestId { get; set; }

        [JsonPropertyName("voucher_code")]
        public string VoucherCode { get; set; } = "";

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }
}
