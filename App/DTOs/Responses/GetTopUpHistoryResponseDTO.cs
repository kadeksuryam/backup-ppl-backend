using System;
using System.Text.Json.Serialization;
using App.Helpers;

namespace App.DTOs.Responses
{
    public class GetTopUpHistoryResponseDTO
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

        public class VoucherDTO
        {
            [JsonPropertyName("id")]
            public uint Id { get; set; }
            [JsonPropertyName("code")]
            public string Code { get; set; } = string.Empty;
        }

        public class BankDTO
        {
            [JsonPropertyName("id")]
            public uint Id { get; set; }
            [JsonPropertyName("name")]
            public string? Name { get; set; }
            [JsonPropertyName("account_number")]
            public long AccountNumber { get; set; }
        }


        public class TopUpHistoryDTO
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
            [JsonPropertyName("amount")]
            public uint Amount { get; set; }
            [JsonPropertyName("method")]
            public string? Method { get; set; }
            [JsonPropertyName("voucher")]
            public VoucherDTO? Voucher { get; set; }
            [JsonPropertyName("bank")]
            public BankDTO? Bank { get; set; }
        }

        [JsonPropertyName("topup_histories")]
        public List<TopUpHistoryDTO> TopUpHistories { get; set; }
        [JsonPropertyName("paging")]
        public PagingDTO Paging { get; set; }
    }
}

