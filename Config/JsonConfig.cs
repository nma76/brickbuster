using System.Text.Json;

namespace brickbuster.Config;

public static class JsonConfig
{
    public static JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
}