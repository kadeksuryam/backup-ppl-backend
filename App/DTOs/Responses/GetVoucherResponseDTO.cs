using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class GetVoucherResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("amount")]
        public uint Amount { get; set; }
        [JsonPropertyName("is_used")]
        public bool IsUsed { get; set; }
    }
}