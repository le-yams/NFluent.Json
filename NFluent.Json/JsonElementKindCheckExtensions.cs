using System.Text.Json;
using NFluent.Extensibility;
using NFluent.Json.Internal;
using NFluent.Kernel;

namespace NFluent.Json;

public static class JsonElementKindCheckExtensions
{
    /// <summary>
    /// Checks that the actual <c>JsonElement</c> has the specified <c>JsonValueKind</c>.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <param name="kind"></param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element has not the specified kind.</exception>
    public static ICheckLink<ICheck<JsonElement>> HasKind(this ICheck<JsonElement> check, JsonValueKind kind)
    {
        var kindStr = JsonValueKindFormatter.Format(kind);
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != kind, $"The element is not {kindStr}.")
            .OnNegate($"The element is {kindStr} whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is a string.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not a string.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsString(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.String, "The element is not a string.")
            .OnNegate("The element is a string whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is a number.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not a number.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsNumber(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Number, "The element is not a number.")
            .OnNegate("The element is a number whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is a boolean.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not a boolean.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsBoolean(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.False && sut.ValueKind != JsonValueKind.True, "The element is not a boolean.")
            .OnNegate("The element is a boolean whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is a <c>false</c> boolean.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not a <c>false</c> boolean.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsFalse(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.False, "The element is not false.")
            .OnNegate("The element is false whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is a <c>true</c> boolean.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not a <c>true</c> boolean.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsTrue(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.True, "The element is not true.")
            .OnNegate("The element is true whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is an array.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not an array.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsArray(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Array, "The element is not an array.")
            .OnNegate("The element is an array whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }

    /// <summary>
    /// Checks that the actual <c>JsonElement</c> is an object.
    /// </summary>
    /// <param name="check">The fluent check to be extended.</param>
    /// <returns>
    /// A check link.
    /// </returns>
    /// <exception cref="FluentCheckException">The actual element is not an object.</exception>
    public static ICheckLink<ICheck<JsonElement>> IsObject(this ICheck<JsonElement> check)
    {
        ExtensibilityHelper.BeginCheck(check)
            .FailWhen(sut => sut.ValueKind != JsonValueKind.Object, "The element is not an object.")
            .OnNegate("The element is an object whereas it must not.").EndCheck();
        return ExtensibilityHelper.BuildCheckLink(check);
    }
}