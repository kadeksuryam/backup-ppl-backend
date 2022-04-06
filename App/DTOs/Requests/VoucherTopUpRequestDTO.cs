using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
    public class VoucherTopUpRequestDTO
    {
        [JsonPropertyName("voucher_code")]
        public string VoucherCode { get; set; } = "";
    }
}
