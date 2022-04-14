using System;
using System.Text.Json.Serialization;
using App.Helpers;

namespace App.DTOs.Responses
{

    public class GetTransactionHistoryResponseDTO
	{

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

        public class TransactionHistoryDTO
        {
            [JsonPropertyName("id")]
            public uint Id { get; set; }
            [JsonPropertyName("created_at")]
            [JsonConverter(typeof(DateTimeJSONConverter))]
            public DateTime? CreatedAt { get; set; }
            [JsonPropertyName("updated_at")]
            [JsonConverter(typeof(DateTimeJSONConverter))]
            public DateTime? UpdatedAt { get; set; }
            [JsonPropertyName("from")]
            public UserDTO? From { get; set; }
            [JsonPropertyName("to")]
            public UserDTO? To { get; set; }
            [JsonPropertyName("amount")]
            public uint Amount { get; set; }
            [JsonPropertyName("status")]
            public string? Status { get; set; }
        }

        [JsonPropertyName("transaction_histories")]
        public List<TransactionHistoryDTO> TransactionHistories { get; set; }
        [JsonPropertyName("paging")]
        public PagingDTO Paging { get; set; }
    }
}

