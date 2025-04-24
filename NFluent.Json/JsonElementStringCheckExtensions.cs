using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementStringCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement string value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasStringValue(this ICheck<JsonElement> check, string expectedValue,
        StringComparison? comparisonType = null)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String,
                "The element is not a string.")
            .FailWhen(sut => !EqualityCheck(comparisonType)(sut.GetString()!, expectedValue),
                $"The element is not equal to the expected value '{expectedValue}'.")
            .OnNegate(
                $"The element is equal to '{expectedValue}' whereas it must not{ComparisonTypeTextAddOn(comparisonType)}.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has the specified string property with the expected value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <param name="comparisonType">One of the enumeration values that specifies how the strings will be compared</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasStringProperty(this ICheck<JsonElement> check, string propertyName,
        string expectedValue, StringComparison? comparisonType = null)
    {
        var kindStr = JsonValueKindFormatter.Format(JsonValueKind.String);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => !sut.TryGetProperty(propertyName, out _),
                $"The '{propertyName}' property is undefined.")
            .FailWhen(
                sut => sut.TryGetProperty(propertyName, out _) &&
                       sut.GetProperty(propertyName).ValueKind != JsonValueKind.String,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .FailWhen(sut => !EqualityCheck(comparisonType)(sut.GetProperty(propertyName).GetString()!, expectedValue),
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate(
                $"The property '{propertyName}' is present and has value equal to '{expectedValue}' whereas it must not{ComparisonTypeTextAddOn(comparisonType)}.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }

    private static Func<string, string, bool> EqualityCheck(StringComparison? comparisonType)
    {
        return comparisonType == null
            ? (value, expected) => value.Equals(expected)
            : (value, expected) => value.Equals(expected, comparisonType.Value);
    }

    private static string ComparisonTypeTextAddOn(StringComparison? comparisonType)
    {
        return comparisonType == null
            ? string.Empty
            : $" (comparison type {comparisonType.Value})";
    }
}