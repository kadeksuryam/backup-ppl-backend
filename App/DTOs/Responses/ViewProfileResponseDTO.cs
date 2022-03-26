using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class ViewProfileResponseDTO
    {
        [JsonPropertyName("id")]
        public uint Id { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
        [JsonPropertyName("balance")]
        public uint Balance { get; set; }
        [JsonPropertyName("exp")]
        public uint EXP { get; set; }
        [JsonPropertyName("level")]
        public string Level { get; set; }

    }
}
