using System.Text.Json;

public static class JsonConfig
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}