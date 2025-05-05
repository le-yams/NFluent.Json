using System.Text.Json;
using Json.More;
using Json.Path;
using NFluent.Json.Exceptions;

namespace NFluent.Json.Extensions;

public static class JsonElementExtensions
{
    public static JsonElement RequireElementAt(this JsonElement element, string path)
    {
        return element.GetElementAt(path)
               ?? throw new JsonException($"Expected at least one element at path '{path}'.");
    }

    public static JsonElement? GetElementAt(this JsonElement element, string path)
    {
        var p = ParsePath(path);

        var result = p.Evaluate(element.AsNode());

        var nodes = result.Matches;

        return nodes.Count switch
        {
            0 => null,
            1 => nodes[0].Value.Deserialize<JsonElement>(),
            _ => throw new JsonException($"Found more than one element at path '{path}'.")
        };
    }

    private static JsonPath ParsePath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new InvalidPathException("Path cannot be null or empty.", nameof(path));
        }

        path = AddJsonPathMandatorySuffixIfMissing(path);

        try
        {
            return JsonPath.Parse(path);
        }
        catch (Exception e)
        {
            throw new InvalidPathException(path, $"Invalid path '{path}'", e);
        }
    }

    private static string AddJsonPathMandatorySuffixIfMissing(string path)
    {
        var firstChar = path[0];
        if (firstChar != '$' && firstChar != '[' && firstChar != '.')
        {
            path = $".{path}";
        }

        firstChar = path[0];
        if (firstChar != '$')
        {
            path = $"${path}";
        }

        return path;
    }
}