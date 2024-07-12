using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementArrayPropertyCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement has the specified array property with the expected value (order matters).
    /// Not that this method uses System.Text.Json deserialization and objects static comparison to check the array items presence.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasArrayProperty<T>(this ICheck<JsonElement> check,
        string propertyName,
        IEnumerable<T> expectedValue)
    {
        var expectedArray = expectedValue.ToArray();
        var kindStr = JsonValueKindFormatter.Format(JsonValueKind.Array);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(
                sut => sut.TryGetProperty(propertyName, out _) &&
                       sut.GetProperty(propertyName).ValueKind != JsonValueKind.Array,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .FailWhen(sut => !sut.GetProperty(propertyName).ArrayEqualTo(expectedArray).Strict,
                $"The property value is not equal to the expected value [{string.Join(",", expectedArray)}].")
            .OnNegate(
                $"The property '{propertyName}' is present and has value [{string.Join(",", expectedArray)}] whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has the specified array property equivalent to the expected value (same elements in any order).
    /// Not that this method uses System.Text.Json deserialization and objects static comparison to check the array items presence.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasArrayPropertyEquivalentTo<T>(this ICheck<JsonElement> check,
        string propertyName,
        IEnumerable<T> expectedValue)
    {
        var expectedArray = expectedValue.ToArray();
        var kindStr = JsonValueKindFormatter.Format(JsonValueKind.Array);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(
                sut => sut.TryGetProperty(propertyName, out _) &&
                       sut.GetProperty(propertyName).ValueKind != JsonValueKind.Array,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .FailWhen(sut => !sut.GetProperty(propertyName).ArrayEqualTo(expectedArray).IgnoringOrder,
                $"The property value is not equal to the expected value [{string.Join(",", expectedArray)}].")
            .OnNegate(
                $"The property '{propertyName}' is present and has value [{string.Join(",", expectedArray)}] whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }
}