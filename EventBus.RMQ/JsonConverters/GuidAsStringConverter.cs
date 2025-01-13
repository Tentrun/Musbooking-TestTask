using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventBus.RMQ.JsonConverters;

public sealed class GuidAsStringConverter : JsonConverter<Guid>
{
    public override Guid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();

        return string.IsNullOrWhiteSpace(value)
            ? Guid.Empty
            : Guid.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, Guid value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}