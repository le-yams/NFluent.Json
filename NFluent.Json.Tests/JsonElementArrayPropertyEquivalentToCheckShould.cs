using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayPropertyEquivalentToCheckShould
{
    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayPropertyEquivalentToWorksOnHavingPropertyWithExpectedNumbers(int[] value,
        IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2" }, new[] { "1", "2" })]
    [InlineData(new[] { "1", "2" }, new[] { "2", "1" })]
    public async Task HasArrayPropertyEquivalentToWorksOnHavingPropertyWithExpectedStrings(string[] value,
        IEnumerable<string> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { true, false, true }, new[] { true, false, true })]
    [InlineData(new[] { true, false, true }, new[] { true, true, false })]
    public async Task HasArrayPropertyEquivalentToWorksOnHavingPropertyWithExpectedBooleans(bool[] value,
        IEnumerable<bool> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2", null }, new[] { "1", "2", null })]
    [InlineData(new[] { "1", "2", null }, new[] { "2", null, "1" })]
    public async Task HasArrayPropertyEquivalentToWorksOnHavingPropertyWithExpectedNullValues(string?[] value,
        IEnumerable<string?> equivalentValue)
    {
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayPropertyEquivalentToWorksOnHavingPropertyWithObjects(int[] ids,
        IEnumerable<int> equivalentIds)
    {
        var value = ids.Select(id => new { id, name = $"name {id}" });
        var equivalentValue = equivalentIds.Select(id => new { id, name = $"name {id}" });
        var json = await TestJson.Element(new { propA = value });

        Check.That(json).HasArrayPropertyEquivalentTo("propA", equivalentValue);
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToFailingWhenPropertyIsUndefined()
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
    public async Task HasArrayPropertyEquivalentToFailingWhenPropertyIsNotAnArray()
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
    public async Task HasArrayPropertyEquivalentToFailingWhenExpectingArrayHasDifferentLength()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[1,2,3]}]");
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToFailingWhenExpectingArrayHasSameLengthButDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var nonExpectedValue = expectedValue.Select(i => i + 1);
        var json = await TestJson.Element(new { propA = nonExpectedValue });

        Check.ThatCode(() => Check.That(json).HasArrayPropertyEquivalentTo("propA", expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[{\"propA\":[2,3]}]");
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToFailingWhenExpectingArrayHasSameLengthButDifferentObjectValues()
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
                "The property value is not equal to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
                "The checked struct:",
                "\t[{\"propA\":[{\"id\":2,\"name\":\"foo\"},{\"id\":3,\"name\":\"bar\"}]}]");
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propB", new[] { 1, 2 });
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToCanBeNegateWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", new[] { 1, 2 });
    }

    [Fact]
    public async Task HasArrayPropertyEquivalentToCanBeNegateWithWrongLength()
    {
        var expectedValue = new[] { 1, 2 };
        var notExpectedValue = expectedValue.Concat(new[] { 3 });
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasArrayPropertyEquivalentTo("propA", expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayPropertyEquivalentToNegationFailingWhenHavingThePropertyWithExpectedValue(int[] value, IEnumerable<int> equivalentValue)
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
    public async Task HasArrayPropertyEquivalentToNegationFailingWhenHavingThePropertyWithExpectedObjectsValue(
        int[] ids, IEnumerable<int> equivalentIds)
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