using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementBoolPropertyCheckShould
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PassWithExpectedValue(bool expectedValue)
    {
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasBoolProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasBoolProperty("propB", true);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.That(json).Not.HasBoolProperty("propA", true);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PassWhenNegatedWithWrongPropertyValue(bool expectedValue)
    {
        var json = await TestJson.Element(new { propA = !expectedValue });

        Check.That(json).Not.HasBoolProperty("propA", expectedValue);
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = true });

        Check.ThatCode(() => Check.That(json).HasBoolProperty("propB", true)).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":true}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNotABoolean()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.ThatCode(() => Check.That(json).HasBoolProperty("propA", true)).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not boolean.",
            "The checked struct:",
            "\t[{\"propA\":42}]");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FailWhenPropertyHasWrongValue(bool expectedValue)
    {
        var json = await TestJson.Element(new { propA = !expectedValue });

        Check.ThatCode(() => Check.That(json).HasBoolProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property value is not equal to the expected value '{expectedValue.ToString().ToLowerInvariant()}'.",
            "The checked struct:",
            $"\t[{{\"propA\":{(!expectedValue).ToString().ToLowerInvariant()}}}]");
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task FailWhenNegatedWithExpectedValue(bool expectedValue)
    {
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasBoolProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value '{expectedValue.ToString().ToLowerInvariant()}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":{expectedValue.ToString().ToLowerInvariant()}}}]");
    }
}