using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementHasElementsCountAtCheckShould
{
    [Theory]
    [InlineData("$.array[*].a")]
    [InlineData(".array[*].a")]
    [InlineData("array[*].a")]
    public async Task PassWhenThereIsTheSpecifiedNumberOfElements(string path)
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });
        Check.That(json).HasElementsCountAt(path, 2);
    }

    [Fact]
    public async Task FailWhenThereIsNotTheSpecifiedNumberOfElements()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });

        Check.ThatCode(() => Check.That(json).HasElementsCountAt("$.array[*].a", 3))
            .IsAFailingCheckWithMessage(
                "",
                "found 2 elements at 'a.c' but was expecting 3.",
                "The checked struct:",
                "\t[{\"array\":[{\"a\":1},{\"a\":2}]}]");
    }

    [Fact]
    public async Task FailWhenNegatedAndThereIsTheSpecifiedNumberOfElements()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });
        Check.ThatCode(() => Check.That(json).Not.HasElementsCountAt("$.array[*].a", 2))
            .IsAFailingCheckWithMessage(
                "",
                "found 2 elements at 'a.c' whereas it must not.",
                "The checked struct:",
                "\t[{\"array\":[{\"a\":1},{\"a\":2}]}]");
    }

    [Fact]
    public async Task PassWhenNegatedAndThereIsNotTheSpecifiedNumberOfElements()
    {
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });
        Check.That(json).Not.HasElementsCountAt("$.array[*].a", 3);
    }
}