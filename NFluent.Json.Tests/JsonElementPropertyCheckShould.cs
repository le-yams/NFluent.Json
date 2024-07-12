using System.Text.Json;
using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementPropertyCheckShould
{
    [Fact]
    public async Task HasPropertyWorksOnHavingProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).HasProperty("propA");
    }

    [Fact]
    public async Task HasPropertyFailingWhenPropertyIsMissing()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasProperty("propB")).IsAFailingCheckWithMessage(
            "",
            "Property 'propB' not found.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasPropertyCanBeNegate()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasProperty("propB");
    }

    [Fact]
    public async Task HasPropertyNegationFailingWhenPropertyIsPresent()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).Not.HasProperty("propA")).IsAFailingCheckWithMessage(
            "",
            "The property 'propA' is present whereas it must not.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasPropertyWithKindWorks()
    {
        var json = await TestJson.Element(new
        {
            stringProp = "",
            intProp = 42,
            nullProp = (string)null!,
            arrayProp = new[] { "" },
            objectProp = new { },
            falseProp = false,
            trueProp = true,
        });

        Check
            .That(json)
            .HasProperty("stringProp", JsonValueKind.String)
            .And.HasProperty("intProp", JsonValueKind.Number)
            .And.HasProperty("undefinedProp", JsonValueKind.Undefined)
            .And.HasProperty("nullProp", JsonValueKind.Null)
            .And.HasProperty("arrayProp", JsonValueKind.Array)
            .And.HasProperty("objectProp", JsonValueKind.Object)
            .And.HasProperty("falseProp", JsonValueKind.False)
            .And.HasProperty("trueProp", JsonValueKind.True);
    }

    [Fact]
    public async Task HasPropertyWithKindFailingWhenPropertyHasWrongKind()
    {
        var json = await TestJson.Element(new { prop = "" });

        Check.ThatCode(() => Check.That(json).HasProperty("prop", JsonValueKind.Number)).IsAFailingCheckWithMessage(
            "",
            "The 'prop' property kind is not number.",
            "The checked struct:",
            "\t[{\"prop\":\"\"}]");
    }

    [Fact]
    public async Task HasPropertyWithKindCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check
            .That(json)
            .Not.HasProperty("stringProp", JsonValueKind.Number)
            .And
            .Not.HasProperty("undefinedProp", JsonValueKind.String);
    }

    [Fact]
    public async Task HasPropertyWithKindNegationFailingWhenPropertyHasWrongKind()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check.ThatCode(() => Check.That(json).Not.HasProperty("stringProp", JsonValueKind.String))
            .IsAFailingCheckWithMessage(
                "",
                "The 'stringProp' property kind is string whereas it must not.",
                "The checked struct:",
                "\t[{\"stringProp\":\"\"}]");
    }
}