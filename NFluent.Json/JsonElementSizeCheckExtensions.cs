using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementSizeCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement size is equal to the specified value. The element must be a string or an array.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element has not the expected size.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasSize(this ICheck<JsonElement> check, int expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String && sut.ValueKind != JsonValueKind.Array,
                "The element is not a string nor an array.")
            .FailWhen(sut =>
                {
                    var length = sut.ValueKind == JsonValueKind.String
                        ? sut.GetString()!.Length
                        : sut.GetArrayLength();
                    return !length.Equals(expectedValue);
                },
                $"The element size is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The element size is '{expectedValue}' whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has a property with its size equal to the specified value.
    /// The property must be a string or an array.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element property has not the expected size.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasPropertyWithSize(this ICheck<JsonElement> check,
        string propertyName, int expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(sut =>
                {
                    sut.TryGetProperty(propertyName, out var property);
                    return property.ValueKind != JsonValueKind.String &&
                           property.ValueKind != JsonValueKind.Array;
                },
                "The property value is not a string nor an array.")
            .FailWhen(sut =>
                {
                    sut.TryGetProperty(propertyName, out var property);
                    var length = property.ValueKind == JsonValueKind.String
                        ? property.GetString()!.Length
                        : property.GetArrayLength();
                    return !length.Equals(expectedValue);
                },
                $"The property size is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property size is '{expectedValue}' whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement is empty. The element must be a string or an array.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not empty.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsEmpty(this ICheck<JsonElement> check)
    {
        return check.HasSize(0);
    }

    /// <summary>
    /// Checks that the actual JsonElement has an empty property. The element property must be a string or an array.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element property is not empty.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasEmptyProperty(this ICheck<JsonElement> check, string propertyName)
    {
        return check.HasPropertyWithSize(propertyName, 0);
    }
}