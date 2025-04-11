using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIsFalseCheckShould
{
    [Fact]
    public async Task PassWhenFalse()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check
            .That(json.GetProperty("falseProp"))
            .IsFalse();
    }

    [Fact]
    public async Task PassWhenNegatedAndTrue()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check
            .That(json.GetProperty("trueProp"))
            .Not.IsFalse();
    }

    [Fact]
    public async Task PassWhenNegatedAndNotABoolean()
    {
        var json = await TestJson.Element(new { stringProp = "42"});

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsFalse();
    }

    [Fact]
    public async Task FailWhenTrue()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check.ThatCode(() => Check.That(json.GetProperty("trueProp")).IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not false.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task FailWhenNotABoolean()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not false.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedAndFalse()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check.ThatCode(() => Check.That(json.GetProperty("falseProp")).Not.IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The element is false whereas it must not.",
                "The checked struct:",
                "\t[False]");
    }
}