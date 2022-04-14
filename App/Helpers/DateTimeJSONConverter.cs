using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace App.Helpers
{
    public class DateTimeJSONConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTime.ParseExact(reader.GetString()!,
                        "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture).ToUniversalTime();

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.ToUniversalTime().ToString(
                    "yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture));
    }
}

