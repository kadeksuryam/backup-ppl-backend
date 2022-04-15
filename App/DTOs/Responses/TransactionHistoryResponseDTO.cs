using System.Text.Json.Serialization;
using App.Helpers;

namespace App.DTOs.Responses
{
    public class TransactionHistoryResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }

        [JsonPropertyName("created_at")]
        [JsonConverter(typeof(DateTimeJSONConverter))]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        [JsonConverter(typeof(DateTimeJSONConverter))]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("sender_user_id")]
        public uint FromUserId { get; set; }

        [JsonPropertyName("receiver_user_id")]
        public uint ToUserId { get; set; }

        [JsonPropertyName("amount")]
        public uint Amount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = "";
    }
}
