using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class VoucherTopUpResponseDTO
    {
        [JsonPropertyName("amount")]
        public uint Amount { get; set; }
    }
}
