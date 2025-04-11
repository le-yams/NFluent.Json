using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasPropertyWithSizeCheckShould
{
    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "foo" });

        Check.ThatCode(() => Check.That(json).HasPropertyWithSize("propB", 3)).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"foo\"}]");
    }

    [Fact]
    public async Task WorksWithStringProperty()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .HasPropertyWithSize("prop", value.Length);
    }

    [Fact]
    public async Task WorksWithArrayProperty()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .HasPropertyWithSize("prop", value.Length);
    }

    [Fact]
    public async Task FailWhenPropertyIsArrayWithDifferentLength()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json).HasPropertyWithSize("prop", value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '2'.",
                "The checked struct:",
                "\t[{\"prop\":[1,2,3]}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsStringWithDifferentLength()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = $"not{value}" });

        Check.ThatCode(() => Check.That(json).HasPropertyWithSize("prop", value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '3'.",
                "The checked struct:",
                "\t[{\"prop\":\"notfoo\"}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsWrongKind()
    {
        var json = await TestJson.Element(new { prop = 42 });

        Check.ThatCode(() => Check.That(json).HasPropertyWithSize("prop", 42))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a string nor an array.",
                "The checked struct:",
                "\t[{\"prop\":42}]");
    }

    [Fact]
    public async Task WorkWhenNegated()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value.Concat(new[] { 3 }) });

        Check
            .That(json)
            .Not.HasPropertyWithSize("prop", value.Length);
    }

    [Fact]
    public async Task HasPropertyWithSizeNegationFailingWithSameSize()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json).Not.HasPropertyWithSize("prop", value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '2' whereas it must not.",
                "The checked struct:",
                "\t[{\"prop\":[1,2]}]");
    }
}