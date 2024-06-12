using System.Text.Json;

namespace ColorMicroservice.Shared.Serialization;

public static class JsonSerializerExtensions
{
    public static string ToJson(this object value, JsonSerializerOptions? settings = null)
        => JsonSerializer.Serialize(value, settings ?? JsonSettings.Default);

    public static T? FromJson<T>(this string value, JsonSerializerOptions? settings = null)
        => JsonSerializer.Deserialize<T?>(value, settings ?? JsonSettings.Default);
}
