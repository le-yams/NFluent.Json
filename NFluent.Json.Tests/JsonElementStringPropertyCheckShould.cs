using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementStringPropertyCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasStringProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassIgnoringCase()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { propA = value.ToLowerInvariant() });

        Check
            .That(json).HasStringProperty("propA", value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasStringProperty("propB", "42");
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.That(json).Not.HasStringProperty("propA", "expectedValue");
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const string expectedValue = "expectedValue";
        var json = await TestJson.Element(new { propA = $"not {expectedValue}" });

        Check.That(json).Not.HasStringProperty("propA", expectedValue);
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasStringProperty("propB", "dummy")).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task FailWhenNotAString()
    {
        var json = await TestJson.Element(new { propA = 42 });

        Check.ThatCode(() => Check.That(json).HasStringProperty("propA", "dummy")).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not string.",
            "The checked struct:",
            "\t[{\"propA\":42}]");
    }

    [Fact]
    public async Task FailWithWrongValue()
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
    public async Task FailWhenNegatedWithExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasStringProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            $"The property 'propA' is present and has value equal to '{expectedValue}' whereas it must not.",
            "The checked struct:",
            $"\t[{{\"propA\":\"{expectedValue}\"}}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithWithCaseInsensitiveValue()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { propA = value.ToLowerInvariant() });
        var expectedValue = value.ToUpperInvariant();

        Check.ThatCode(() => Check.That(json).Not
                .HasStringProperty("propA", expectedValue, StringComparison.OrdinalIgnoreCase))
            .IsAFailingCheckWithMessage(
                "",
                $"The property 'propA' is present and has value equal to '{expectedValue}' whereas it must not (comparison type OrdinalIgnoreCase).",
                "The checked struct:",
                $"\t[{{\"propA\":\"{value}\"}}]");
    }
}