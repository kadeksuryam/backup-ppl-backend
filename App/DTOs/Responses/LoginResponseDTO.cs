using System.Text.Json.Serialization;

namespace App.DTOs.Responses
{
    public class LoginResponseDTO
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("username")]
        public string UserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("jwt_token")]
        public string Token { get; set; }

    }
}
