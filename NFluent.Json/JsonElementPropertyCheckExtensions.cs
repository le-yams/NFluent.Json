using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementPropertyCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement has the specified property.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasProperty(this ICheck<JsonElement> check, string propertyName)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false, $"Property '{propertyName}' not found.")
            .OnNegate($"The property '{propertyName}' is present whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has the specified property with the specified kind.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="kind"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasProperty(this ICheck<JsonElement> check, string propertyName,
        JsonValueKind kind)
    {
        var kindStr = JsonValueKindFormatter.Format(kind);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false && kind != JsonValueKind.Undefined,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) && sut.GetProperty(propertyName).ValueKind != kind,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .OnNegate($"The '{propertyName}' property kind is {kindStr} whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }
}