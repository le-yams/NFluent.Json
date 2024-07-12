using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

internal record JsonWithNullableProperties(int? intProp, string? stringProp, bool? boolProp);

public class JsonElementNullPropertyCheckShould
{
    [Fact]
    public async Task HasNullPropertyWorksOnHavingNullProperty()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(null, null, null));

        Check.That(json).HasNullProperty("intProp");
        Check.That(json).HasNullProperty("stringProp");
        Check.That(json).HasNullProperty("boolProp");
    }

    [Fact]
    public async Task HasNullPropertyFailingWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(null, null, null));

        Check.ThatCode(() => Check.That(json).HasNullProperty("undefinedProp")).IsAFailingCheckWithMessage(
            "",
            "The 'undefinedProp' property is undefined.",
            "The checked struct:",
            "\t[{\"intProp\":null,\"stringProp\":null,\"boolProp\":null}]");
    }

    [Fact]
    public async Task HasNullPropertyFailingWhenPropertyIsNotNull()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(42, "foo", true));

        Check.ThatCode(() => Check.That(json).HasNullProperty("intProp")).IsAFailingCheckWithMessage(
            "",
            "The 'intProp' property is not null.",
            "The checked struct:",
            "\t[{\"intProp\":42,\"stringProp\":\"foo\",\"boolProp\":true}]");
        Check.ThatCode(() => Check.That(json).HasNullProperty("stringProp")).IsAFailingCheckWithMessage(
            "",
            "The 'stringProp' property is not null.",
            "The checked struct:",
            "\t[{\"intProp\":42,\"stringProp\":\"foo\",\"boolProp\":true}]");
        Check.ThatCode(() => Check.That(json).HasNullProperty("boolProp")).IsAFailingCheckWithMessage(
            "",
            "The 'boolProp' property is not null.",
            "The checked struct:",
            "\t[{\"intProp\":42,\"stringProp\":\"foo\",\"boolProp\":true}]");
    }

    [Fact]
    public async Task HasNullPropertyCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(null, null, null));

        Check.That(json).Not.HasNullProperty("undefinedProp");
    }

    [Fact]
    public async Task HasNullPropertyCanBeNegateWithNonNullProperty()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(42, "foo", true));

        Check.That(json).Not.HasNullProperty("intProp");
        Check.That(json).Not.HasNullProperty("stringProp");
        Check.That(json).Not.HasNullProperty("boolProp");
    }

    [Fact]
    public async Task HasNullPropertyNegationFailingWhenHavingTheNullProperty()
    {
        var json = await TestJson.Element(new JsonWithNullableProperties(null, null, null));
    
        Check.ThatCode(() => Check.That(json).Not.HasNullProperty("intProp")).IsAFailingCheckWithMessage(
            "",
            $"The property 'intProp' is present and is null whereas it must not.",
            "The checked struct:",
            "\t[{\"intProp\":null,\"stringProp\":null,\"boolProp\":null}]");
    }
}