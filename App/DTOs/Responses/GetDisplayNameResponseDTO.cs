using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class GetDisplayNameResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
    }
}
