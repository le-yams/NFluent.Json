using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIntValueCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasIntValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasIntValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue.ToString() });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasIntValue(expectedValue);
    }

    [Fact]
    public async Task FailWithWrongValue()
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
    public async Task FailWhenNotANumber()
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
    public async Task FailWhenNegatedWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasIntValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is equal to {expectedValue} whereas it must not.",
                "The checked struct:",
                $"\t[{expectedValue}]");
    }
}