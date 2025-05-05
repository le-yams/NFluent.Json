using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasSingleElementAtCheckShould
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

        Check.That(json).HasSingleElementAt(path);
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

        Check.ThatCode(() => Check.That(json).HasSingleElementAt("a.c")).IsAFailingCheckWithMessage(
            "",
            "No element found at 'a.c'.",
            "The checked struct:",
            "\t[{\"a\":{\"b\":42}}]");
    }

    [Fact]
    public async Task FailWhenThereIsMoreThanOneElement()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });

        Check.ThatCode(() => Check.That(json).HasSingleElementAt("array[*].a")).IsAFailingCheckWithMessage(
            "",
            "Found more than one element at 'array[*].a'.",
            "The checked struct:",
            "\t[{\"array\":[{\"a\":1},{\"a\":2}]}]");
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

        Check.That(json).Not.HasSingleElementAt("a.c");
    }

    [Fact]
    public async Task PassWhenNegatedAndThereIsMoreThanOneElement()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });

        Check.That(json).Not.HasSingleElementAt("array[*].a");
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

        Check.ThatCode(() => Check.That(json).Not.HasSingleElementAt("a.b"))
            .IsAFailingCheckWithMessage(
                "",
                "A single element is found at 'a.b' whereas it must not.",
                "The checked struct:",
                "\t[{\"a\":{\"b\":42}}]");
    }
}