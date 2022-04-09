using App.Helpers.FormatValidator;
using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class TopUpHistoryResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = "";

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = "";

        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("method")]
        public string Method { get; set; } = "";

        [JsonPropertyName("bank_request_id")]
        public uint BankRequestId { get; set; }

        [JsonPropertyName("voucher_code")]
        public string VoucherCode { get; set; } = "";
    }
}
