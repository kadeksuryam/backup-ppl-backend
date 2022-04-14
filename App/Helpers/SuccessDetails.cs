using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Helpers
{
    public class SuccessDetails
    {
        [JsonPropertyName("data")]
        public object? Data { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
