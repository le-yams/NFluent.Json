using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementValueCheckExtensions
{
    /// <summary>
    /// Checks that the actual JsonElement value is null.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not null.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasNullValue(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Null, $"The property value is not null.")
            .OnNegate($"The property value is null whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement string value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasStringValue(this ICheck<JsonElement> check, string expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String,
                "The property value is not a string.")
            .FailWhen(sut => !sut.GetString()!.Equals(expectedValue),
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property value is equal to '{expectedValue}' whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

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
                "The property value is not a number.")
            .FailWhen(sut => sut.GetInt32() != expectedValue,
                $"The property value is not equal to the expected value {expectedValue}.")
            .OnNegate($"The property value is equal to {expectedValue} whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement long value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasLongValue(this ICheck<JsonElement> check, long expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Number,
                "The property value is not a number.")
            .FailWhen(sut => sut.GetInt64() != expectedValue,
                $"The property value is not equal to the expected value {expectedValue}.")
            .OnNegate($"The property value is equal to {expectedValue} whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement boolean value is equal to the specified value.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasBoolValue(this ICheck<JsonElement> check, bool expectedValue)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.True && sut.ValueKind != JsonValueKind.False,
                "The property value is not a boolean.")
            .FailWhen(sut => sut.GetBoolean() != expectedValue,
                $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property value is equal to '{expectedValue}' whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement boolean value is true.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is false.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasTrueValue(this ICheck<JsonElement> check)
    {
        return check.HasBoolValue(true);
    }

    /// <summary>
    /// Checks that the actual JsonElement boolean value is false.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is true.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasFalseValue(this ICheck<JsonElement> check)
    {
        return check.HasBoolValue(false);
    }

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
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String || !Guid.TryParse(sut.GetString(), out _), "The property value is not a Guid.")
            .FailWhen(sut => sut.GetGuid() != expectedValue, $"The property value is not equal to the expected value '{expectedValue}'.")
            .OnNegate($"The property value is equal to '{expectedValue}' whereas it must not.")
            .EndCheck();

        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement array value is equal to the specified value (order matters).
    /// Not that this method uses System.Text.Json deserialization and objects static comparison to check the array items presence.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equal to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasArrayValue<T>(this ICheck<JsonElement> check, IEnumerable<T> expectedValue)
    {
        var expectedArray = expectedValue.ToArray();
        var expectedStr = JsonSerializer.Serialize(expectedArray).Replace("{", "{{").Replace("}", "}}");
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Array,
                "The property value is not an array.")
            .FailWhen(sut => !sut.ArrayEqualTo(expectedArray).Strict,
                $"The property value is not equal to the expected value {expectedStr}.")
            .OnNegate($"The property value is equal to {expectedStr} whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual JsonElement array value is equivalent to the specified enumerable (same elements in any order).
    /// Not that this method uses System.Text.Json deserialization and objects static comparison to check the array items presence.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="expectedValue"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element value is not equivalent to the specified one.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasArrayValueEquivalentTo<T>(this ICheck<JsonElement> check,
        IEnumerable<T?> expectedValue) where T : notnull
    {
        var expectedArray = expectedValue.ToArray();
        var expectedStr = JsonSerializer.Serialize(expectedArray).Replace("{", "{{").Replace("}", "}}");

        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Array,
                "The property value is not an array.")
            .FailWhen(sut => !sut.ArrayEqualTo(expectedArray).IgnoringOrder,
                $"The property value is not equivalent to the expected value {expectedStr}.")
            .OnNegate($"The property value is equivalent to {expectedStr} whereas it must not.")
            .EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }
}