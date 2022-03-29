using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Helpers
{
    public class SuccessDetails
    {
        [JsonPropertyName("status_code")]
        public int StatusCode { get; set; }
        [JsonPropertyName("data")]
        public Object Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
