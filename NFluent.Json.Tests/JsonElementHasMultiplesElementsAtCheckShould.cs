using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasMultipleElementsAtCheckShould
{
    [Theory]
    [InlineData("$.array[*].a")]
    [InlineData(".array[*].a")]
    [InlineData("array[*].a")]
    public async Task PassWhenThereIsMoreThanOneElement(string path)
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });
        Check.That(json).HasMultipleElementsAt(path);
    }

    [Fact]
    public async Task FailWhenThereIsOnlyOneElement()
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.ThatCode(() => Check.That(json).HasMultipleElementsAt("a.b")).IsAFailingCheckWithMessage(
            "",
            "Only one element found at 'a.b'.",
            "The checked struct:",
            "\t[{\"a\":{\"b\":42}}]");
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

        Check.ThatCode(() => Check.That(json).HasMultipleElementsAt("a.c")).IsAFailingCheckWithMessage(
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

        Check.That(json).Not.HasMultipleElementsAt("a.c");
    }

    [Fact]
    public async Task PassWhenNegatedAndThereIsAnElement()
    {
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = 42
            }
        });

        Check.That(json).Not.HasMultipleElementsAt("a.b");
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

        Check.ThatCode(() => Check.That(json).Not.HasMultipleElementsAt("array[*].a"))
            .IsAFailingCheckWithMessage(
                "",
                "Multiple elements found at 'array[*].a' whereas it must not.",
                "The checked struct:",
                "\t[{\"array\":[{\"a\":1},{\"a\":2}]}]");
    }
}