using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementBoolPropertyCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement has the specified boolean property with the expected value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasBoolProperty(this ICheck<JsonElement> check, string propertyName,
        bool expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
        .FailWhen(sut => sut.TryGetProperty(propertyName, out _) && sut.GetProperty(propertyName).ValueKind != JsonValueKind.True && sut.GetProperty(propertyName).ValueKind != JsonValueKind.False,
            $"The '{propertyName}' property kind is not boolean.")
        .FailWhen(sut => sut.GetProperty(propertyName).GetBoolean()!= expectedValue,
            $"The property value is not equal to the expected value '{expectedValue.ToString().ToLowerInvariant()}'.")
        .OnNegate($"The property '{propertyName}' is present and has value '{expectedValue.ToString().ToLowerInvariant()}' whereas it must not.")
        .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }
}