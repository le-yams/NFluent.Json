using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIsTrueCheckShould
{
    [Fact]
    public async Task PassWhenTrue()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check
            .That(json.GetProperty("trueProp"))
            .IsTrue();
    }

    [Fact]
    public async Task PassWhenNegatedAndFalse()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check
            .That(json.GetProperty("falseProp"))
            .Not.IsTrue();
    }

    [Fact]
    public async Task PassWhenNegatedAndNotABoolean()
    {
        var json = await TestJson.Element(new { stringProp = "42"});

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsTrue();
    }

    [Fact]
    public async Task FailWhenFalse()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check.ThatCode(() => Check.That(json.GetProperty("falseProp")).IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not true.",
                "The checked struct:",
                "\t[False]");
    }

    [Fact]
    public async Task FailWhenNotABoolean()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not true.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedAndTrue()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check.ThatCode(() => Check.That(json.GetProperty("trueProp")).Not.IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is true whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }
}