using System.Text.Json;

namespace NFluent.Json.Extensions;

public static class HttpContentExtensions
{
    public static async Task<JsonElement> ReadJsonRootElementAsync(this HttpContent? content)
    {
        return (await content.ReadJsonDocumentAsync()).RootElement;
    }

    private static async Task<JsonDocument> ReadJsonDocumentAsync(this HttpContent? content)
    {
        content = content ?? throw new ArgumentException("http content is null");

        var stringContent = await content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(stringContent))
        {
            throw new ArgumentException("http content is empty");
        }

        return JsonDocument.Parse(stringContent);
    }
}