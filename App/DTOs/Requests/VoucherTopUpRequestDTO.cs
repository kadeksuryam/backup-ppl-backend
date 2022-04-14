using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
    public class VoucherTopUpRequestDTO
    {
        [JsonPropertyName("user_id")]
        public uint UserId { get; set; }

        [JsonPropertyName("voucher_code")]
        public string VoucherCode { get; set; } = "";
    }
}
