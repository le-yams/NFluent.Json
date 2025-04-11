using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementLongValueCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const long expectedValue = long.MaxValue;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasLongValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasLongValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue.ToString() });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasLongValue(expectedValue);
    }

    [Fact]
    public async Task FailWithWrongValue()
    {
        const long expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value 42.",
                "The checked struct:",
                "\t[43]");
    }

    [Fact]
    public async Task FailWhenNotANumber()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not a number.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to 42 whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }
}