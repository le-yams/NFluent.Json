using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementBoolValueCheckShould
{
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
    public async Task HasBoolValueFailingWhenPropertyIsWrongKind()
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
    public async Task HasBoolValueNegationFailingWhenPropertyIsOfSpecifiedKind()
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
    public async Task HasTrueValueFailingWhenPropertyIsWrongKind()
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
    public async Task HasTrueValueNegationFailingWhenPropertyIsOfSpecifiedKind()
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
    public async Task HasFalseValueFailingWhenPropertyIsWrongKind()
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
    public async Task HasFalseValueNegationFailingWhenPropertyIsOfSpecifiedKind()
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