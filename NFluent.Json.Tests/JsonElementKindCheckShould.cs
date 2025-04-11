using System.Text.Json;
using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementKindCheckShould
{
    [Fact]
    public async Task PassWithExpectedKind()
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
            .That(json.GetProperty("stringProp"))
            .HasKind(JsonValueKind.String);
        Check
            .That(json.GetProperty("intProp"))
            .HasKind(JsonValueKind.Number);
        Check
            .That(json.GetProperty("nullProp"))
            .HasKind(JsonValueKind.Null);
        Check
            .That(json.GetProperty("arrayProp"))
            .HasKind(JsonValueKind.Array);
        Check
            .That(json.GetProperty("objectProp"))
            .HasKind(JsonValueKind.Object);
        Check
            .That(json.GetProperty("falseProp"))
            .HasKind(JsonValueKind.False);
        Check
            .That(json.GetProperty("trueProp"))
            .HasKind(JsonValueKind.True);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.HasKind(JsonValueKind.Number);
    }

    [Fact]
    public async Task PassWithAString()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check
            .That(json.GetProperty("stringProp"))
            .IsString();
    }

    [Fact]
    public async Task PassWhenNegatedWithNotAString()
    {
        var json = await TestJson.Element(new { intProp = 42 });

        Check
            .That(json.GetProperty("intProp"))
            .Not.IsString();
    }

    [Fact]
    public async Task PassWithANumber()
    {
        var json = await TestJson.Element(new { numberProp = 42 });

        Check
            .That(json.GetProperty("numberProp"))
            .IsNumber();
    }

    [Fact]
    public async Task PassWhenNegatedWithNotANumber()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsNumber();
    }

    [Fact]
    public async Task PassWhenNegatedWithNotAnArray()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.IsArray();
    }

    [Fact]
    public async Task PassWithABoolean()
    {
        var json = await TestJson.Element(new { trueProp = true, falseProp = false });

        Check
            .That(json.GetProperty("trueProp"))
            .IsBoolean();
        Check
            .That(json.GetProperty("falseProp"))
            .IsBoolean();
    }

    [Fact]
    public async Task PassWithAnArray()
    {
        var json = await TestJson.Element(new { arrayProp = new[] { "" } });

        Check
            .That(json.GetProperty("arrayProp"))
            .IsArray();
    }

    [Fact]
    public async Task PassWithAnObject()
    {
        var json = await TestJson.Element(new { objProp = new { } });

        Check
            .That(json.GetProperty("objProp"))
            .IsObject();
    }

    [Fact]
    public async Task PassWhenNegatedWithNotAnObject()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.IsObject();
    }

    [Fact]
    public async Task FailWithWrongKind()
    {
        var json = await TestJson.Element(new { prop = "" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasKind(JsonValueKind.Number))
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not number.",
                "The checked struct:",
                "\t[]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedKind()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).Not.HasKind(JsonValueKind.String))
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is string whereas it must not.",
                "The checked struct:",
                "\t[]");
    }

    [Fact]
    public async Task FailWithNotAString()
    {
        var json = await TestJson.Element(new { prop = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsString())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not a string.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithAString()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).Not.IsString())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is a string whereas it must not.",
                "The checked struct:",
                "\t[]");
    }

    [Fact]
    public async Task FailWithNotANumber()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsNumber())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not a number.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithANumber()
    {
        var json = await TestJson.Element(new { numberProp = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("numberProp")).Not.IsNumber())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is a number whereas it must not.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWithNotABoolean()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsBoolean())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not a boolean.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task PassWhenNegatedWithNotABoolean()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsBoolean();
    }

    [Fact]
    public async Task FailWhenNegatedWithABoolean()
    {
        var json = await TestJson.Element(new { boolProp = true });

        Check.ThatCode(() => Check.That(json.GetProperty("boolProp")).Not.IsBoolean())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is a boolean whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task FailWithNotAnArray()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsArray())
            .IsAFailingCheckWithMessage(
                "",
                "The property is not an array.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithAnArray()
    {
        var json = await TestJson.Element(new { arrayProp = new[] { "" } });

        Check.ThatCode(() => Check.That(json.GetProperty("arrayProp")).Not.IsArray())
            .IsAFailingCheckWithMessage(
                "",
                "The property is an array whereas it must not.",
                "The checked struct:",
                "\t[[\"\"]]");
    }

    [Fact]
    public async Task FailWithNotAnObject()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsObject())
            .IsAFailingCheckWithMessage(
                "",
                "The property is not an object.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithAnObject()
    {
        var json = await TestJson.Element(new { objProp = new { } });

        Check.ThatCode(() => Check.That(json.GetProperty("objProp")).Not.IsObject())
            .IsAFailingCheckWithMessage(
                "",
                "The property is an object whereas it must not.",
                "The checked struct:",
                "\t[{}]");
    }
}