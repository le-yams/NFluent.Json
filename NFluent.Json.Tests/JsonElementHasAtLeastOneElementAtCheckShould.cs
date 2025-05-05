using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasAtLeastOneElementAtCheckShould
{
    [Theory]
    [InlineData("$.a.b")]
    [InlineData(".a.b")]
    [InlineData("a.b")]
    public async Task PassWhenThereIsAnElement(string path)
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.That(json).HasAtLeastOneElementAt(path);
    }

    [Fact]
    public async Task PassWhenThereIsMoreThanOneElement()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });
        Check.That(json).HasAtLeastOneElementAt("array[*].a");
    }

    [Fact]
    public async Task FailWhenThereIsNoElement()
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.ThatCode(() => Check.That(json).HasAtLeastOneElementAt("a.c")).IsAFailingCheckWithMessage(
            "",
            "No element found at 'a.c'.",
            "The checked struct:",
            "\t[{\"a\":{\"b\":42}}]");
    }

    [Fact]
    public async Task PassWhenNegatedAndThereIsNoElement()
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.That(json).Not.HasAtLeastOneElementAt("a.c");
    }

    [Fact]
    public async Task FailWhenNegatedAndThereIsAnElement()
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.ThatCode(() => Check.That(json).Not.HasAtLeastOneElementAt("a.b"))
            .IsAFailingCheckWithMessage(
                "",
                "At least one element found at 'a.b' whereas it must not.",
                "The checked struct:",
                "\t[{\"a\":{\"b\":42}}]");
    }

    [Fact]
    public async Task FailWhenNegatedAndThereIsMoreThanOneElement()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });

        Check.ThatCode(() => Check.That(json).Not.HasAtLeastOneElementAt("array[*].a"))
            .IsAFailingCheckWithMessage(
                "",
                "At least one element found at 'array[*].a' whereas it must not.",
                "The checked struct:",
                "\t[{\"array\":[{\"a\":1},{\"a\":2}]}]");
    }
}