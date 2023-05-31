using System.Text.Json;
using NFluent;
using NFluent.Helpers;
using NFluent.Json;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementKindCheckShould
{
    [Fact]
    public async Task HasKindWorks()
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
    public async Task HasKindFailingWhenPropertyIsWrongKing()
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
    public async Task HasKindCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.HasKind(JsonValueKind.Number);
    }

    [Fact]
    public async Task HasKindNegationFailingWhenPropertyIsOfSpecifiedKing()
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
    public async Task IsStringWorks()
    {
        var json = await TestJson.Element(new { stringProp = "" });

        Check
            .That(json.GetProperty("stringProp"))
            .IsString();
    }

    [Fact]
    public async Task IsStringFailingWhenPropertyIsWrongKing()
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
    public async Task IsStringCanBeNegate()
    {
        var json = await TestJson.Element(new { intProp = 42 });

        Check
            .That(json.GetProperty("intProp"))
            .Not.IsString();
    }

    [Fact]
    public async Task IsStringNegationFailingWhenPropertyIsOfSpecifiedKing()
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
    public async Task IsNumberWorks()
    {
        var json = await TestJson.Element(new { numberProp = 42 });

        Check
            .That(json.GetProperty("numberProp"))
            .IsNumber();
    }

    [Fact]
    public async Task IsNumberFailingWhenPropertyIsWrongKing()
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
    public async Task IsNumberCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsNumber();
    }

    [Fact]
    public async Task IsNumberNegationFailingWhenPropertyIsOfSpecifiedKing()
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
    public async Task IsBooleanWorks()
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
    public async Task IsBooleanFailingWhenPropertyIsWrongKing()
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
    public async Task IsBooleanCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "42" });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsBoolean();
    }

    [Fact]
    public async Task IsBooleanNegationFailingWhenPropertyIsOfSpecifiedKing()
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
    public async Task IsFalseWorks()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check
            .That(json.GetProperty("falseProp"))
            .IsFalse();
    }

    [Fact]
    public async Task IsFalseFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { stringProp = "42", trueProp = true });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not false.",
                "The checked struct:",
                "\t[42]");
        
        Check.ThatCode(() => Check.That(json.GetProperty("trueProp")).IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not false.",
                "The checked struct:",
                "\t[True]");
    }

    [Fact]
    public async Task IsFalseCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "42", trueProp = true });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsFalse();
        Check
            .That(json.GetProperty("trueProp"))
            .Not.IsFalse();
    }

    [Fact]
    public async Task IsFalseNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { falseProp = false });

        Check.ThatCode(() => Check.That(json.GetProperty("falseProp")).Not.IsFalse())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is false whereas it must not.",
                "The checked struct:",
                "\t[False]");
    }

    [Fact]
    public async Task IsTrueWorks()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check
            .That(json.GetProperty("trueProp"))
            .IsTrue();
    }

    [Fact]
    public async Task IsTrueFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { stringProp = "42", falseProp = false });

        Check.ThatCode(() => Check.That(json.GetProperty("stringProp")).IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not true.",
                "The checked struct:",
                "\t[42]");
        
        Check.ThatCode(() => Check.That(json.GetProperty("falseProp")).IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is not true.",
                "The checked struct:",
                "\t[False]");
    }

    [Fact]
    public async Task IsTrueCanBeNegate()
    {
        var json = await TestJson.Element(new { stringProp = "42", falseProp = false });

        Check
            .That(json.GetProperty("stringProp"))
            .Not.IsTrue();
        Check
            .That(json.GetProperty("falseProp"))
            .Not.IsTrue();
    }

    [Fact]
    public async Task IsTrueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { trueProp = true });

        Check.ThatCode(() => Check.That(json.GetProperty("trueProp")).Not.IsTrue())
            .IsAFailingCheckWithMessage(
                "",
                "The property kind is true whereas it must not.",
                "The checked struct:",
                "\t[True]");
    }
    
    [Fact]
    public async Task IsArrayWorks()
    {
        var json = await TestJson.Element(new { arrayProp = new[]{""} });

        Check
            .That(json.GetProperty("arrayProp"))
            .IsArray();
    }

    [Fact]
    public async Task IsArrayFailingWhenPropertyIsWrongKing()
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
    public async Task IsArrayCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.IsArray();
    }

    [Fact]
    public async Task IsArrayNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { arrayProp = new[]{""} });

        Check.ThatCode(() => Check.That(json.GetProperty("arrayProp")).Not.IsArray())
            .IsAFailingCheckWithMessage(
                "",
                "The property is an array whereas it must not.",
                "The checked struct:",
                "\t[[\"\"]]");
    }
    
    [Fact]
    public async Task IsObjectWorks()
    {
        var json = await TestJson.Element(new { objProp = new {} });

        Check
            .That(json.GetProperty("objProp"))
            .IsObject();
    }

    [Fact]
    public async Task IsObjectFailingWhenPropertyIsWrongKing()
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
    public async Task IsObjectCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.IsObject();
    }

    [Fact]
    public async Task IsObjectNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { objProp = new {} });

        Check.ThatCode(() => Check.That(json.GetProperty("objProp")).Not.IsObject())
            .IsAFailingCheckWithMessage(
                "",
                "The property is an object whereas it must not.",
                "The checked struct:",
                "\t[{}]");
    }
}