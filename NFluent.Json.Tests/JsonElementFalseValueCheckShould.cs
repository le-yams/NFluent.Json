using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementFalseValueCheckShould
{
    [Fact]
    public async Task PassWhenValueIsFalse()
    {
        var json = await TestJson.Element(new { prop = false });

        Check
            .That(json.GetProperty("prop"))
            .HasFalseValue();
    }

    [Fact]
    public async Task PassWhenNegatedWithTrueValue()
    {
        var json = await TestJson.Element(new { prop = true });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasFalseValue();
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { prop = 42 });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasFalseValue();
    }

    [Fact]
    public async Task FailWhenValueIsTrue()
    {
        var json = await TestJson.Element(new { prop = true });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value 'False'.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task FailWhenNotABoolean()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithFalseValue()
    {
        var json = await TestJson.Element(new { prop = false });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasFalseValue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to 'False' whereas it must not.",
                "The checked struct:",
                "\t[False]");
    }
}