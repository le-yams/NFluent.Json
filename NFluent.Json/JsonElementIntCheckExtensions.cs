﻿using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementIntCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement int value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasIntValue(this ICheck<JsonElement> check, int expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Number,
                "The element is not a number.")
            .FailWhen(sut => sut.GetInt32() != expectedValue,
                $"The element is not equal to the expected value {expectedValue}.")
            .OnNegate($"The element is equal to {expectedValue} whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement has the specified int property with the expected value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="propertyName">the name of the property to check</param>
    /// <param name="expectedValue">the value of the property to check</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual value has no property with the specified name and value.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasIntProperty(this ICheck<JsonElement> check, string propertyName,
        int expectedValue)
    {
        var kindStr = JsonValueKindFormatter.Format(JsonValueKind.Number);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.TryGetProperty(propertyName, out _) == false,
                $"The '{propertyName}' property is undefined.")
            .FailWhen(
                sut => sut.TryGetProperty(propertyName, out _) &&
                       sut.GetProperty(propertyName).ValueKind != JsonValueKind.Number,
                $"The '{propertyName}' property kind is not {kindStr}.")
            .FailWhen(sut => sut.GetProperty(propertyName).GetInt32() != expectedValue,
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property '{propertyName}' is present and has value '{expectedValue}' whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }
}