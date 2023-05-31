using System.Text.Json;

namespace NFluent.Json.Internal;

internal static class JsonValueKindFormatter
{
    public static string Format(JsonValueKind kind)
    {
        return kind switch
        {
            JsonValueKind.Undefined => "undefined",
            JsonValueKind.Object => "object",
            JsonValueKind.Array => "array",
            JsonValueKind.String => "string",
            JsonValueKind.Number => "number",
            JsonValueKind.True => "boolean",
            JsonValueKind.False => "boolean",
            JsonValueKind.Null => "null",
            _ => throw new ArgumentOutOfRangeException(nameof(kind), kind, null)
        };
    }
}