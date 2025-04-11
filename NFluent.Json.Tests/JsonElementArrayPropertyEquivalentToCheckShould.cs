using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayPropertyEquivalentToCheckShould
{
    [Theory]
    [InlineData(new[] { true, false, true }, new[] { true, false, true })]
    [InlineData(new[] { true, false, true }, new[] { true, true, false })]
    public async Task PassWithBooleans(bool[] value,
        IEnumerable<bool> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWithObjects(int[] ids,
        IEnumerable<int> equivalentIds)
    {
        var value = ids.Select(id => new { id, name = $"name {id}" });
        var equivalentValue = equivalentIds.Select(id => new { id, name = $"name {id}" });
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2", null }, new[] { "1", "2", null })]
    [InlineData(new[] { "1", "2", null }, new[] { "2", null, "1" })]
    public async Task PassWithNullValues(string?[] value,
        IEnumerable<string?> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWithNumbers(int[] value,
        IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2" }, new[] { "1", "2" })]
    [InlineData(new[] { "1", "2" }, new[] { "2", "1" })]
    public async Task PassWithStrings(string[] value,
        IEnumerable<string> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propB", new[] { 1, 2 });
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", new[] { 1, 2 });
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongSize()
    {
        var expectedValue = new[] { 1, 2 };
        var notExpectedValue = expectedValue.Concat(new[] { 3 });
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task PassWhenNegatedWithDifferentValues(int[] expectedValue, IEnumerable<int> notEquivalentValue)
    {
        var json = await TestJson.Element(new { prop = notEquivalentValue });

        Check
            .That(json)
            .Not.HasArrayPropertyEquivalentTo("prop", expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task PassWhenNegatedWithDifferentObjectValues(int[] expectedIds, IEnumerable<int> notEquivalentIds)
    {
        var value = expectedIds.Select(i => new { id = i }).ToArray();
        var notEquivalentValue = notEquivalentIds.Select(i => new { id = i }).ToArray();

        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json)
            .Not.HasArrayPropertyEquivalentTo("prop", notEquivalentValue);
    }

    [Fact]
    public async Task FailWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propB", new[] { 1, 2 }))
            .IsAFailingCheckWithMessage(
                "",
                "The 'propB' property is undefined.",
                "The checked struct:",
                "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task FailWhenPropertyIsNotAnArray()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", new[] { 1, 2 }))
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

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equivalent to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[1,2,3]}]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var nonExpectedValue = expectedValue.Select(i => i + 1);
        var json = await TestJson.Element(new { propA = nonExpectedValue });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equivalent to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[2,3]}]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentObjectValues()
    {
        var expectedValue = new[]
        {
            new { id = 1, name = "foo" },
            new { id = 2, name = "bar" }
        };
        var nonExpectedValue = expectedValue.Select(i => i with { id = i.id + 1 });
        var json = await TestJson.Element(new { propA = nonExpectedValue });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equivalent to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
                "The checked struct:",
                "\t[{\"propA\":[{\"id\":2,\"name\":\"foo\"},{\"id\":3,\"name\":\"bar\"}]}]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task FailWhenNegatedWithEquivalentValues(int[] value, IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.ThatCode(() => Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", equivalentValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property 'propA' is present and has value [{string.Join(",", equivalentValue)}] whereas it must not.",
                "The checked struct:",
                $"\t[{{\"propA\":[{string.Join(",", value)}]}}]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task FailWhenNegatedWithEquivalentObjectValues(int[] ids, IEnumerable<int> equivalentIds)
    {
        var value = ids.Select(i => new { id = i }).ToArray();
        var valueStr = string.Join(',', value.Select(v => $"{{\"id\":{v.id}}}"));
        var equivalentValue = equivalentIds.Select(i => new { id = i }).ToArray();
        var equivalentValueStr = string.Join(',', equivalentValue.Select(v => $"{{\"id\":{v.id}}}"));
        var json = await TestJson.Element(new { propA = value });

        Check.ThatCode(() => Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", equivalentValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property 'propA' is present and has value [{equivalentValueStr}] whereas it must not.",
                "The checked struct:",
                $"\t[{{\"propA\":[{valueStr}]}}]");
    }
}