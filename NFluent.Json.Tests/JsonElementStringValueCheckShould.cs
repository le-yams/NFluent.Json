using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementStringValueCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = expectedValue });

        Check
            .That(json.GetProperty("stringProp"))
            .HasStringValue(expectedValue);
    }

    [Fact]
    public async Task PassIgnoringCase()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { stringProp = value.ToLowerInvariant() });

        Check
            .That(json.GetProperty("stringProp"))
            .HasStringValue(value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { prop = $"not {expectedValue}" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasStringValue("");
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        const int expectedValue = 42;
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasStringValue(expectedValue.ToString());
    }

    [Fact]
    public async Task FailWithWrongValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = $"not {expectedValue}" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value '42'.",
                "The checked struct:",
                "\t[not 42]");
    }

    [Fact]
    public async Task FailWhenNotAString()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { numberProp = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("numberProp")).HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not a string.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        const string expectedValue = "42";
        var json = await TestJson.Element(new { stringProp = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).Not.HasStringValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to '42' whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithWithCaseInsensitiveValue()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { stringProp = value.ToLowerInvariant() });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).Not.HasStringValue(value.ToUpperInvariant(), StringComparison.OrdinalIgnoreCase))
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to 'FOO' whereas it must not (comparison type OrdinalIgnoreCase).",
                "The checked struct:",
                "\t[foo]");
    }
}