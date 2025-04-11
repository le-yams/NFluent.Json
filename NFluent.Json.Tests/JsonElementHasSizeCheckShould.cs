using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasSizeCheckShould
{
    [Fact]
    public async Task PassWithString()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasSize(value.Length);
    }

    [Fact]
    public async Task PassWithArray()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasSize(value.Length);
    }

    [Fact]
    public async Task PassWhenNegatedWithArrayWrongSize()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value.Concat(new[] { 3 }) });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasSize(value.Length);
    }

    [Fact]
    public async Task PassWhenNegatedWithStringWrongSize()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = value+"bar" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasSize(value.Length);
    }

    [Fact]
    public async Task FailWithArrayWrongSize()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasSize(value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '2'.",
                "The checked struct:",
                "\t[[1,2,3]]");
    }

    [Fact]
    public async Task FailWithStringWrongSize()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = $"not{value}" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasSize(value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is not equal to the expected value '3'.",
                "The checked struct:",
                "\t[notfoo]");
    }

    [Fact]
    public async Task FailWhenNotAnArrayNorAString()
    {
        var json = await TestJson.Element(new { prop = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasSize(42))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a string nor an array.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedArraySize()
    {
        var value = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasSize(value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '2' whereas it must not.",
                "The checked struct:",
                "\t[[1,2]]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedStringSize()
    {
        const string value = "foo";
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasSize(value.Length))
            .IsAFailingCheckWithMessage(
                "",
                "The property size is '3' whereas it must not.",
                "The checked struct:",
                "\t[foo]");
    }
}