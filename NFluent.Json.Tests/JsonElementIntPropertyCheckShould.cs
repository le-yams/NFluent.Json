using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIntPropertyCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasIntProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasIntProperty("propB", 42);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const int expectedValue = int.MaxValue;
        const int notExpectedValue = expectedValue - 1;
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasIntProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasIntProperty("propA", 42);
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasIntProperty("propB", 42)).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task FailWithWrongValue()
    {
        const int expectedValue = 42;
        const int notExpectedValue = expectedValue + 1;
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.ThatCode(() => Check.That(json).HasIntProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property value is not equal to the expected value '{expectedValue}'.",
            "The checked struct:",
            $"\t[{{\"propA\":{notExpectedValue}}}]");
    }

    [Fact]
    public async Task FailWhenNotANumber()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json).HasIntProperty("propA", 42)).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not number.",
            "The checked struct:",
            "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasIntProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value '{expectedValue}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":{expectedValue}}}]");
    }
}