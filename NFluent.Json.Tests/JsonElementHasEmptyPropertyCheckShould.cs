using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasEmptyPropertyCheckShould
{
    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "foo" });

        Check.ThatCode(() => Check.That(json).HasEmptyProperty("propB")).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"foo\"}]");
    }

    [Fact]
    public async Task PassWithEmptyStringProperty()
    {
        const string value = "";
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .HasEmptyProperty("prop");
    }

    [Fact]
    public async Task PassWithEmptyArrayProperty()
    {
        var value = Array.Empty<int>();
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .HasEmptyProperty("prop");
    }

    [Fact]
    public async Task FailWhenPropertyIsNonEmptyArray()
    {
        var json = await TestJson.Element(new { prop = new[] { 1, 2 } });

        Check.ThatCode(() => Check.That(json).HasEmptyProperty("prop"))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '0'.",
                "The checked struct:",
                "\t[{\"prop\":[1,2]}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNonEmptyString()
    {
        var json = await TestJson.Element(new { prop = "foo" });

        Check.ThatCode(() => Check.That(json).HasEmptyProperty("prop"))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '0'.",
                "The checked struct:",
                "\t[{\"prop\":\"foo\"}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsWrongKind()
    {
        var json = await TestJson.Element(new { prop = 0 });

        Check.ThatCode(() => Check.That(json).HasEmptyProperty("prop"))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a string nor an array.",
                "The checked struct:",
                "\t[{\"prop\":0}]");
    }

    [Fact]
    public async Task PassWhenNegatedWithNonEmptyArrayProperty()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .Not.HasEmptyProperty("prop");
    }

    [Fact]
    public async Task PassWhenNegatedWithNonEmptyStringProperty()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .Not.HasEmptyProperty("prop");
    }

    [Fact]
    public async Task FailWhenNegatedWithEmptyArrayProperty()
    {
        var json = await TestJson.Element(new { prop = Array.Empty<int>() });

        Check.ThatCode(() => Check.That(json).Not.HasEmptyProperty("prop"))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '0' whereas it must not.",
                "The checked struct:",
                "\t[{\"prop\":[]}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithEmptyStringProperty()
    {
        var json = await TestJson.Element(new { prop = "" });

        Check.ThatCode(() => Check.That(json).Not.HasEmptyProperty("prop"))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '0' whereas it must not.",
                "The checked struct:",
                "\t[{\"prop\":\"\"}]");
    }
}