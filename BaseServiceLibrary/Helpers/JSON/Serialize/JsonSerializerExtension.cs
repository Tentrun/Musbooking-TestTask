using System.Text.Json;
using System.Text.Json.Serialization;

namespace BaseServiceLibrary.Helpers.JSON.Serialize;

public static class JsonSerializerExtension
{
    private static JsonSerializerOptions _options;
    static JsonSerializerExtension()
    {
        _options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = ReferenceHandler.IgnoreCycles
        };
        _options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public static JsonSerializerOptions Options => _options;

    public static string Serialize<T>(this T obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }

    public static T? Deserialize<T>(this string value)
    {
        return JsonSerializer.Deserialize<T>(value, _options);
    }
}