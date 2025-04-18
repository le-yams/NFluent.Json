using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementGuidCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement Guid value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasGuidValue(this ICheck<JsonElement> check, Guid expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String || !Guid.TryParse(sut.GetString(), out _),
                "The element is not a Guid.")
            .FailWhen(sut => sut.GetGuid() != expectedValue,
                $"The element is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The element is equal to '{expectedValue}' whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has the specified Guid property with the expected value
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>A check link.</returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasGuidProperty(this ICheck<JsonElement> check, string propertyName,
        Guid expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(PropertyIsNotFound(propertyName), $"The '{propertyName}' property is undefined.")
            .FailWhen(PropertyHasNotExpectedValueKind(propertyName, JsonValueKind.String),
                $"The '{propertyName}' property kind is not Guid.")
            .FailWhen(PropertyHasNotExpectedValue(propertyName, expectedValue),
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property '{propertyName}' is present and has value '{expectedValue}' whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }

    private static Func<JsonElement, bool> PropertyIsNotFound(string propertyName)
    {
        return jsonElement => jsonElement.TryGetProperty(propertyName, out _) == false;
    }

    private static Func<JsonElement, bool> PropertyHasNotExpectedValueKind(string propertyName,
        JsonValueKind expectedValueKind)
    {
        return jsonElement => jsonElement.TryGetProperty(propertyName, out _)
                              && (jsonElement.GetProperty(propertyName).ValueKind != expectedValueKind
                                  || !Guid.TryParse(jsonElement.GetProperty(propertyName).GetString(), out _));
    }

    private static Func<JsonElement, bool> PropertyHasNotExpectedValue(string propertyName, Guid expectedValue)
    {
        return jsonElement =>
            jsonElement.GetProperty(propertyName).GetString()
                ?.Equals(expectedValue.ToString(), StringComparison.OrdinalIgnoreCase) == false;
    }
}