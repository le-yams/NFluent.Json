using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementLongPropertyCheckShould
{
    [Fact]
    public async Task HasLongPropertyWorksOnHavingPropertyWithExpectedValue()
    {
        const long expectedValue = 42;
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasLongProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasLongPropertyFailingWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasLongProperty("propB", 42)).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasLongPropertyFailingWhenPropertyIsNotANumber()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json).HasLongProperty("propA", 42)).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not number.",
            "The checked struct:",
            "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task HasLongPropertyFailingWhenPropertyHasWrongValue()
    {
        const long expectedValue = long.MaxValue;
        const long notExpectedValue = expectedValue - 1;
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.ThatCode(() => Check.That(json).HasLongProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property value is not equal to the expected value '{expectedValue}'.",
            "The checked struct:",
            $"\t[{{\"propA\":{notExpectedValue}}}]");
    }

    [Fact]
    public async Task HasLongPropertyCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasLongProperty("propB", 42);
    }

    [Fact]
    public async Task HasLongPropertyCanBeNegateWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasLongProperty("propA", 42);
    }

    [Fact]
    public async Task HasLongPropertyCanBeNegateWithWrongPropertyValue()
    {
        const int expectedValue = 42;
        const int notExpectedValue = expectedValue + 1;
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasLongProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasLongPropertyNegationFailingWhenHavingThePropertyWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasLongProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value '{expectedValue}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":{expectedValue}}}]");
    }
}