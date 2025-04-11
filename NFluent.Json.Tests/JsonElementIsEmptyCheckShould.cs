using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementIsEmptyCheckShould
{
    [Fact]
    public async Task WorkWithString()
    {
        const string value = "";
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .IsEmpty();
    }

    [Fact]
    public async Task WorkWithArray()
    {
        var value = Array.Empty<int>();
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .IsEmpty();
    }

    [Fact]
    public async Task FailWhenPropertyIsNonEmptyArray()
    {
        var json = await TestJson.Element(new { prop = new[] { 1, 2 } });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsEmpty())
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '0'.",
                "The checked struct:",
                "\t[[1,2]]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNonEmptyString()
    {
        var json = await TestJson.Element(new { prop = "foo" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsEmpty())
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '0'.",
                "The checked struct:",
                "\t[foo]");
    }

    [Fact]
    public async Task FailWhenPropertyIsWrongKind()
    {
        var json = await TestJson.Element(new { prop = 0 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).IsEmpty())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a string nor an array.",
                "The checked struct:",
                "\t[0]");
    }

    [Fact]
    public async Task WorkWhenNegated()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .Not.IsEmpty();
    }

    [Fact]
    public async Task FailWhenNegatedAndPropertyActuallyIsEmpty()
    {
        var json = await TestJson.Element(new { prop = Array.Empty<int>() });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.IsEmpty())
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '0' whereas it must not.",
                "The checked struct:",
                "\t[[]]");
    }
}