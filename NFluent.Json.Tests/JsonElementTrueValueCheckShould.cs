using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementTrueValueCheckShould
{
    [Fact]
    public async Task PassWhenValueIsTrue()
    {
        var json = await TestJson.Element(new { prop = true });

        Check
            .That(json.GetProperty("prop"))
            .HasTrueValue();
    }

    [Fact]
    public async Task PassWhenNegatedWithFalseValue()
    {
        var json = await TestJson.Element(new { prop = false });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasTrueValue();
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { prop = 42 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasTrueValue();
    }

    [Fact]
    public async Task FailWhenValueIsFalse()
    {
        var json = await TestJson.Element(new { prop = false });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value 'True'.",
                "The checked struct:",
                "\t[False]");
    }

    [Fact]
    public async Task FailWhenNotABoolean()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithTrueValue()
    {
        var json = await TestJson.Element(new { prop = true });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasTrueValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to 'True' whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }
}