using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayValueEquivalentToCheckShould
{
    [Theory]
    [InlineData(new[] { true, false, true }, new[] { true, false, true })]
    [InlineData(new[] { true, false, true }, new[] { true, true, false })]
    public async Task PassWithBooleans(bool[] value, IEnumerable<bool> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWithObjects(int[] ids, IEnumerable<int> equivalentIds)
    {
        var value = ids.Select(id => new { id, name = $"name {id}" });
        var equivalentValue = equivalentIds.Select(id => new { id, name = $"name {id}" });
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2", null }, new[] { "1", "2", null })]
    [InlineData(new[] { "1", "2", null }, new[] { "2", null, "1" })]
    public async Task PassWithNullValues(string?[] value, IEnumerable<string?> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWithNumbers(int[] value, IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2" }, new[] { "1", "2" })]
    [InlineData(new[] { "1", "2" }, new[] { "2", "1" })]
    public async Task PassWithStrings(string[] value, IEnumerable<string> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = "42" });

        Check.That(json.GetProperty("prop")).Not.HasArrayValueEquivalentTo(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongSize()
    {
        var json = await TestJson.Element(new { prop = new[] { 1, 1, 1 } });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValueEquivalentTo(new[] { 1, 2 });
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task PassWhenNegatedWithDifferentValues(int[] expectedValue, IEnumerable<int> notEquivalentValue)
    {
        var json = await TestJson.Element(new { prop = notEquivalentValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValueEquivalentTo(expectedValue);
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
            .That(json.GetProperty("prop"))
            .Not.HasArrayValueEquivalentTo(notEquivalentValue);
    }

    [Fact]
    public async Task FailWhenElementIsNotAnArray()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("propA")).HasArrayValueEquivalentTo(new[] { 1, 2 }))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not an array.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentSize()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equivalent to the expected value [1,2].",
                "The checked struct:",
                "\t[[1,2,3]]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task FailWhenArrayHasDifferentValues(int[] expectedValue, int[] actualValue)
    {
        var json = await TestJson.Element(new { prop = actualValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is not equivalent to the expected value [{string.Join(",", expectedValue)}].",
                "The checked struct:",
                $"\t[[{string.Join(",", actualValue)}]]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task FailWhenArrayHasDifferentObjectValues(int[] expectedIds, int[] actualIds)
    {
        var actualValue = actualIds.Select(i => new { id = i }).ToArray();
        var actualValueStr = string.Join(',', actualValue.Select(v => $"{{\"id\":{v.id}}}"));
        var expectedValue = expectedIds.Select(i => new { id = i }).ToArray();
        var expectedValueStr = string.Join(',', expectedValue.Select(v => $"{{\"id\":{v.id}}}"));
        var json = await TestJson.Element(new { prop = actualValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is not equivalent to the expected value [{expectedValueStr}].",
                "The checked struct:",
                $"\t[[{actualValueStr}]]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task FailWhenNegatedWithEquivalentValues(int[] value, IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValueEquivalentTo(equivalentValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is equivalent to [{string.Join(",", equivalentValue)}] whereas it must not.",
                "The checked struct:",
                $"\t[[{string.Join(",", value)}]]");
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
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValueEquivalentTo(equivalentValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is equivalent to [{equivalentValueStr}] whereas it must not.",
                "The checked struct:",
                $"\t[[{valueStr}]]");
    }
}