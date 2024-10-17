using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementGuidPropertyCheckShould
{
    [Fact]
    public async Task HasGuidPropertyWorksOnHavingPropertyWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasGuidProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasGuidPropertyFailingWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propB", Guid.NewGuid())).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasGuidPropertyFailingWhenPropertyIsNotAGuid()
    {
        var json = await TestJson.Element(new { propA = "42" });
        var expectedValue = Guid.NewGuid();

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not string.",
            "The checked struct:",
            "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task HasGuidPropertyFailingWhenPropertyHasWrongValue()
    {
        var expectedValue = Guid.NewGuid();
        var notExpectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property value is not equal to the expected value '{expectedValue}'.",
            "The checked struct:",
            $"\t[{{\"propA\":\"{notExpectedValue}\"}}]");
    }

    [Fact]
    public async Task HasGuidPropertyCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasGuidProperty("propB", Guid.NewGuid());
    }

    [Fact]
    public async Task HasGuidPropertyCanBeNegateWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = Guid.NewGuid() });

        Check.That(json).Not.HasGuidProperty("propA", Guid.NewGuid());
    }

    [Fact]
    public async Task HasGuidPropertyCanBeNegateWithWrongPropertyValue()
    {
        var expectedValue = Guid.NewGuid();
        var notExpectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasGuidProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasGuidPropertyNegationFailingWhenHavingThePropertyWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasGuidProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value '{expectedValue}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":\"{expectedValue}\"}}]");
    }
}