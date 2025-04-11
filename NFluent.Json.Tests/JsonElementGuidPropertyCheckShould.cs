using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementGuidPropertyCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasGuidProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasGuidProperty("propB", Guid.NewGuid());
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        var expectedValue = Guid.NewGuid();
        var notExpectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasGuidProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { propA = "not a guid" });

        Check.That(json).Not.HasGuidProperty("propA", Guid.NewGuid());
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propB", Guid.NewGuid()))
            .IsAFailingCheckWithMessage(
                "",
                "The 'propB' property is undefined.",
                "The checked struct:",
                "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNotAGuid()
    {
        var json = await TestJson.Element(new { propA = "42" });
        var expectedValue = Guid.NewGuid();

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The 'propA' property kind is not Guid.",
                "The checked struct:",
                "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task FailWithWrongValue()
    {
        var expectedValue = Guid.NewGuid();
        var notExpectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.ThatCode(() => Check.That(json).HasGuidProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is not equal to the expected value '{expectedValue}'.",
                "The checked struct:",
                $"\t[{{\"propA\":\"{notExpectedValue}\"}}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasGuidProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property 'propA' is present and has value '{expectedValue}' whereas it must not.",
                "The checked struct:",
                $"\t[{{\"propA\":\"{expectedValue}\"}}]");
    }
}