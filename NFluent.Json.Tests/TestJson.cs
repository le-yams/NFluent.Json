using System.Net.Http.Json;
using System.Text.Json;

namespace NFluent.Json.Tests;

public static class TestJson
{
    public static async Task<JsonElement> Element(object obj)
    {
        var json = await JsonDocument.ParseAsync(await JsonContent.Create(obj).ReadAsStreamAsync());
        return json.RootElement;
    }
}