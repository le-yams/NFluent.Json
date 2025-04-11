using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayPropertyCheckShould
{
    [Fact]
    public async Task PassWithBooleans()
    {
        var expectedValue = new[] { true, false, true };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWithObjects()
    {
        var expectedValue = new[]
        {
            new { id = 1, name = "1" },
            new { id = 2, name = "2" },
            new { id = 3, name = "3" }
        };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWithNullValues()
    {
        var expectedValue = new int?[] { 1, null };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWithNumbers()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWithStrings()
    {
        var expectedValue = new[] { "1", "2" };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasArrayProperty("propB", new[] { 1, 2 });
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasArrayProperty("propA", new[] { 1, 2 });
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongSize()
    {
        var expectedValue = new[] { 1, 2 };
        var notExpectedValue = expectedValue.Concat(new[] { 3 });
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasArrayProperty("propA", expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWhenNegatedWithDifferentValues(int[] expectedValue, IEnumerable<int> notEquivalentValue)
    {
        var json = await TestJson.Element(new { prop = notEquivalentValue });

        Check
            .That(json)
            .Not.HasArrayProperty("prop", expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWhenNegatedWithDifferentObjectValues(int[] expectedIds, IEnumerable<int> notEquivalentIds)
    {
        var value = expectedIds.Select(i => new { id = i }).ToArray();
        var notEquivalentValue = notEquivalentIds.Select(i => new { id = i }).ToArray();

        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .Not.HasArrayProperty("prop", notEquivalentValue);
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propB", new[] { 1, 2 })).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNotAnArray()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", new[] { 1, 2 }))
            .IsAFailingCheckWithMessage(
                "",
                "The 'propA' property kind is not array.",
                "The checked struct:",
                "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentSize()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[1,2,3]}]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue.Reverse() });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[2,1]}]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentObjectValues()
    {
        var expectedValue = new[]
        {
            new { id = 1, name = "foo" },
            new { id = 2, name = "bar" }
        };
        var json = await TestJson.Element(new { propA = expectedValue.Reverse() });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
                "The checked struct:",
                "\t[{\"propA\":[{\"id\":2,\"name\":\"bar\"},{\"id\":1,\"name\":\"foo\"}]}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasArrayProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property 'propA' is present and has value [1,2] whereas it must not.",
                "The checked struct:",
                "\t[{\"propA\":[1,2]}]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedObjectsValue()
    {
        var expectedValue = new[] { new { id = 1 }, new { id = 2 } };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasArrayProperty("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property 'propA' is present and has value [{\"id\":1},{\"id\":2}] whereas it must not.",
                "The checked struct:",
                "\t[{\"propA\":[{\"id\":1},{\"id\":2}]}]");
    }
}