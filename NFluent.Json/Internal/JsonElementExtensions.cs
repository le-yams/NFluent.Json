using System.Text.Json;

namespace NFluent.Json.Internal;

internal static class JsonElementExtensions
{
    public static JsonArrayEqualityCheck<T> ArrayEqualTo<T>(this JsonElement element, T[] other)
    {
        return new JsonArrayEqualityCheck<T>(element, other);
    }
}