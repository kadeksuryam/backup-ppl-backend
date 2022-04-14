using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class TransactionHistoryResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; } = "";

        [JsonPropertyName("updated_at")]
        public string UpdatedAt { get; set; } = "";

        [JsonPropertyName("receiver_user_id")]
        public uint ToUserId { get; set; }

        [JsonPropertyName("amount")]
        public uint Amount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }
}
