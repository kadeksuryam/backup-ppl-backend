using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace App.DTOs.Requests
{
    public class UpdateTopUpRequestStatusRequestDTO
    {
        [JsonPropertyName("id")]
        [Required]
        public uint Id { get; set; }
        [JsonPropertyName("status")]
        [Required]
        public string? Status { get; set; }

    }
}
