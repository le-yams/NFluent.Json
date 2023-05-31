using NFluent;
using NFluent.Helpers;
using NFluent.Json;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementStringPropertyCheckShould
{
    [Fact]
    public async Task HasStringPropertyWorksOnHavingPropertyWithExpectedValue()
    {
        var expectedValue = "42";
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasStringProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasStringPropertyFailingWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasStringProperty("propB", "dummy")).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasStringPropertyFailingWhenPropertyIsNotAString()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.ThatCode(() => Check.That(json).HasStringProperty("propA", "dummy")).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not string.",
            "The checked struct:",
            "\t[{\"propA\":42}]");
    }

    [Fact]
    public async Task HasStringPropertyFailingWhenPropertyHasWrongValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { propA = $"not {expectedValue}" });

        Check.ThatCode(() => Check.That(json).HasStringProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property value is not equal to the expected value '42'.",
            "The checked struct:",
            "\t[{\"propA\":\"not 42\"}]");
    }

    [Fact]
    public async Task HasStringPropertyCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasStringProperty("propB", "42");
    }

    [Fact]
    public async Task HasStringPropertyCanBeNegateWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.That(json).Not.HasStringProperty("propA", "expectedValue");
    }

    [Fact]
    public async Task HasStringPropertyCanBeNegateWithWrongPropertyValue()
    {
        const string expectedValue = "expectedValue";
        var json = await TestJson.Element(new { propA = $"not {expectedValue}" });

        Check.That(json).Not.HasStringProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasStringPropertyNegationFailingWhenHavingThePropertyWithExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasStringProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value '{expectedValue}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":\"{expectedValue}\"}}]");
    }
}