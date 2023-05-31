using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementStringPropertyCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement has the specified string property with the expected value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasStringProperty(this ICheck<JsonElement> check, string propertyName,
        string expectedValue)
    {
        var kindStr = JsonValueKindFormatter.Format(JsonValueKind.String);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) && sut.GetProperty(propertyName).ValueKind != JsonValueKind.String,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .FailWhen(sut => !sut.GetProperty(propertyName).GetString()!.Equals(expectedValue),
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property '{propertyName}' is present and has value '{expectedValue}' whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }
}