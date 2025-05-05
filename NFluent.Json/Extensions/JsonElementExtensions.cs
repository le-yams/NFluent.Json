using System.Text.Json;
using Json.More;
using Json.Path;
using NFluent.Json.Exceptions;

namespace NFluent.Json.Extensions;

public static class JsonElementExtensions
{
    /// <summary>
    /// Gets the element at the specified JSON path query or fails if no or more than one element found.
    /// </summary>
    /// <param name="element">The JSON element to search in.</param>
    ///  <param name="query">The JSON path query.</param>
    /// <returns> The found JSON element.</returns>
    /// <exception cref="JsonException">if no or more than one element is found</exception>
    public static JsonElement GetRequiredElementAt(this JsonElement element, string query)
    {
        return element.GetElementAt(query)
               ?? throw new JsonException($"No element found at '{query}'.");
    }

    /// <summary>
    /// Gets the element at the specified JSON path query or null if no element is found.
    /// </summary>
    /// <param name="element">The JSON element to search in.</param>
    ///  <param name="query">The JSON path query.</param>
    /// <returns> The found JSON element, or null if not found.</returns>
    /// <exception cref="JsonException">if more than one element is found</exception>
    public static JsonElement? GetElementAt(this JsonElement element, string query)
    {
        var p = ParseQuery(query);

        var result = p.Evaluate(element.AsNode());

        var nodes = result.Matches;

        return nodes.Count switch
        {
            0 => null,
            1 => nodes[0].Value.Deserialize<JsonElement>(),
            _ => throw new JsonException($"Found more than one element at '{query}'.")
        };
    }

    private static JsonPath ParseQuery(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            throw new InvalidPathException("JSON path query cannot be null or empty.", nameof(query));
        }

        query = AddQueryMandatorySuffixIfMissing(query);

        try
        {
            return JsonPath.Parse(query);
        }
        catch (Exception e)
        {
            throw new InvalidPathException(query, $"Invalid JSON path query '{query}'", e);
        }
    }

    private static string AddQueryMandatorySuffixIfMissing(string location)
    {
        var firstChar = location[0];
        if (firstChar != '$' && firstChar != '[' && firstChar != '.')
        {
            location = $".{location}";
        }

        firstChar = location[0];
        if (firstChar != '$')
        {
            location = $"${location}";
        }

        return location;
    }
}