using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Helpers
{
    public class ErrorDetails
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("errors")]
        public string Message { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
