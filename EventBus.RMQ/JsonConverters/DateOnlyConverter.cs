using System.Text.Json;
using System.Text.Json.Serialization;

namespace EventBus.RMQ.JsonConverters;

public sealed class DateOnlyConverter : JsonConverter<DateOnly?>
{
    private readonly string _serializationFormat;

    public DateOnlyConverter() : this(null)
    {
    }

    public DateOnlyConverter(string? serializationFormat)
    {
        _serializationFormat = serializationFormat ?? "yyyy-MM-dd";
    }

    public override DateOnly? Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrWhiteSpace(value)
            ? null
            : DateOnly.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, DateOnly? value,
        JsonSerializerOptions options)
    {
        writer.WriteStringValue(value?.ToString(_serializationFormat));
    }
}