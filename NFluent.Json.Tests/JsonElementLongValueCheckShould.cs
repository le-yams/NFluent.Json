using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementLongValueCheckShould
{
    [Fact]
    public async Task HasLongValueWorks()
    {
        const long expectedValue = long.MaxValue;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasLongValue(expectedValue);
    }

    [Fact]
    public async Task HasLongValueFailingWhenPropertyIsNotExpectedValue()
    {
        const long expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 42.",
                "The checked struct:",
                "\t[43]");
    }

    [Fact]
    public async Task HasLongValueFailingWhenPropertyIsWrongKind()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a number.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasLongValueCanBeNegate()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue + 1 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasLongValue(expectedValue);
    }

    [Fact]
    public async Task HasLongValueNegationFailingWhenPropertyIsOfSpecifiedKind()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasLongValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 42 whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }
}