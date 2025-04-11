using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementBoolValueCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasBoolValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check
            .That(json.GetProperty("propA"))
            .Not.HasBoolValue(true);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const bool expectedValue = true;
        var json = await TestJson.Element(new { prop = !expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasBoolValue(expectedValue);
    }

    [Fact]
    public async Task FailWhithWrongValue()
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
    public async Task FailWhenNotABoolean()
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
    public async Task FailWhenNegatedWithExpectedValue()
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
}