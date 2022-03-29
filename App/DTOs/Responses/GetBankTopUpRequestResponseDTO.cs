using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{

    public class GetBankTopUpRequestResponseDTO
    {
        public class BankDTO
        {
            [JsonPropertyName("id")]
            public uint Id { get; set; }

            [JsonPropertyName("name")]
            public string? Name { get; set; }

            [JsonPropertyName("account_number")]
            public long AccountNumber { get; set; }
        }

        public class UserDTO
        {
            [JsonPropertyName("id")]
            public uint Id { get; set; }
            [JsonPropertyName("username")]
            public string UserName { get; set; } = string.Empty;
            [JsonPropertyName("email")]
            public string Email { get; set; } = string.Empty;
            [JsonPropertyName("balance")]
            public uint Balance { get; set; }
        }

        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("created_at")]
        public string? CreatedAt { get; set; }
        [JsonPropertyName("updated_at")]
        public string? UpdatedAt { get; set; }
        [JsonPropertyName("expired_date")]
        public string? ExpiredDate { get; set; }
        [JsonPropertyName("amount")]
        public int Amount { get; set; }

        [JsonPropertyName("bank")]
        public BankDTO? Bank { get; set; }

        [JsonPropertyName("from")]
        public UserDTO? From { get; set; }

        [JsonPropertyName("status")]
        public string? Status { get; set; }
    }
}
