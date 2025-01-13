using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventBus.RMQ.JsonConverters;

public class DateTimeOffsetNullableConverter : JsonConverter<DateTimeOffset?>
{
    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var val = reader.GetString();

        if (val is null)
            return null;

        val = val.Replace("%2B", "+");

        return DateTimeOffset.Parse(val);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value is null)
            return;

        writer.WriteStringValue(value.Value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
    }
}