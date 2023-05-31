using NFluent;
using NFluent.Helpers;
using NFluent.Json;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementValueCheckShould
{
    [Fact]
    public async Task HasNullValueWorks()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check
            .That(json.GetProperty("nullProp"))
            .HasNullValue();
    }

    [Fact]
    public async Task HasNullValueFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not null.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasNullValueCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasNullValue();
    }

    [Fact]
    public async Task HasNullValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check.ThatCode(() => Check.That(json.GetProperty("nullProp")).Not.HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is null whereas it must not.",
                "The checked struct:",
                "\t[]");
    }

    [Fact]
    public async Task HasStringValueWorks()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = expectedValue });

        Check
            .That(json.GetProperty("stringProp"))
            .HasStringValue(expectedValue);
    }

    [Fact]
    public async Task HasStringValueFailingWhenPropertyIsNotExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = $"not {expectedValue}" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value '42'.",
                "The checked struct:",
                "\t[not 42]");
    }

    [Fact]
    public async Task HasStringValueFailingWhenPropertyIsWrongKing()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { numberProp = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("numberProp")).HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a string.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasStringValueCanBeNegate()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { prop = $"not {expectedValue}" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasStringValue("");
    }

    [Fact]
    public async Task HasStringValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).Not.HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to '42' whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasIntValueWorks()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasIntValue(expectedValue);
    }

    [Fact]
    public async Task HasIntValueFailingWhenPropertyIsNotExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasIntValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 42.",
                "The checked struct:",
                "\t[43]");
    }

    [Fact]
    public async Task HasIntValueFailingWhenPropertyIsWrongKing()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasIntValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a number.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasIntValueCanBeNegate()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasIntValue(expectedValue);
    }

    [Fact]
    public async Task HasIntValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasIntValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 42 whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasBoolValueWorks()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasBoolValue(expectedValue);
    }

    [Fact]
    public async Task HasBoolValueFailingWhenPropertyIsNotExpectedValue()
    {
        const bool expectedValue = false;
        var json = await TestJson.Element(new { prop = !expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasBoolValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 'False'.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task HasBoolValueFailingWhenPropertyIsWrongKing()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasBoolValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasBoolValueCanBeNegate()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = !expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasBoolValue(expectedValue);
    }

    [Fact]
    public async Task HasBoolValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasBoolValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 'True' whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task HasTrueValueWorks()
    {
        var json = await TestJson.Element(new { prop = true });

        Check
            .That(json.GetProperty("prop"))
            .HasTrueValue();
    }

    [Fact]
    public async Task HasTrueValueFailingWhenPropertyIsFalse()
    {
        var json = await TestJson.Element(new { prop = false });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 'True'.",
                "The checked struct:",
                "\t[False]");
    }

    [Fact]
    public async Task HasTrueValueFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasTrueValueCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = false });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasTrueValue();
    }

    [Fact]
    public async Task HasTrueValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { prop = true });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 'True' whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task HasFalseValueWorks()
    {
        var json = await TestJson.Element(new { prop = false });

        Check
            .That(json.GetProperty("prop"))
            .HasFalseValue();
    }

    [Fact]
    public async Task HasFalseValueFailingWhenPropertyIsTrue()
    {
        var json = await TestJson.Element(new { prop = true });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 'False'.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task HasFalseValueFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasFalseValueCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = true });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasFalseValue();
    }

    [Fact]
    public async Task HasFalseValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { prop = false });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 'False' whereas it must not.",
                "The checked struct:",
                "\t[False]");
    }
}