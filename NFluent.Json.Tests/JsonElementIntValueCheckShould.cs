using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIntValueCheckShould
{
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
    public async Task HasIntValueFailingWhenPropertyIsWrongKind()
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
    public async Task HasIntValueNegationFailingWhenPropertyIsOfSpecifiedKind()
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
}