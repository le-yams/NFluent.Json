using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementNullPropertyCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement has the specified null property.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name or has a not null value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasNullProperty(this ICheck<JsonElement> check, string propertyName)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) && sut.GetProperty(propertyName).ValueKind != JsonValueKind.Null,
                $"The '{propertyName}' property is not null.")
            .OnNegate($"The property '{propertyName}' is present and is null whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }
}