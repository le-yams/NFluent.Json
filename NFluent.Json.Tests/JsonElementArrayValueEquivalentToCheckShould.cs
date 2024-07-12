using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayValueEquivalentToCheckShould
{
    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayValueEquivalentToWorksWithNumbers(int[] value, IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2" }, new[] { "1", "2" })]
    [InlineData(new[] { "1", "2" }, new[] { "2", "1" })]
    public async Task HasArrayValueEquivalentToWorksWithStrings(string[] value, IEnumerable<string> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { true, false, true }, new[] { true, false, true })]
    [InlineData(new[] { true, false, true }, new[] { true, true, false })]
    public async Task HasArrayValueEquivalentToWorksWithBooleans(bool[] value, IEnumerable<bool> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { "1", "2", null }, new[] { "1", "2", null })]
    [InlineData(new[] { "1", "2", null }, new[] { "2", null, "1" })]
    public async Task HasArrayValueEquivalentToWorksWithNullValues(string?[] value, IEnumerable<string?> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayValueEquivalentToWorksWithObjects(int[] ids, IEnumerable<int> equivalentIds)
    {
        var value = ids.Select(id => new { id, name = $"name {id}" });
        var equivalentValue = equivalentIds.Select(id => new { id, name = $"name {id}" });
        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValueEquivalentTo(equivalentValue);
    }

    [Fact]
    public async Task HasArrayValueEquivalentToFailingWhenPropertyIsArrayWithDifferentLength()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[[1,2,3]]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task HasArrayValueEquivalentToFailingWhenPropertyIsArrayWithSameLengthButDifferentValues(
        int[] expectedValue, int[] actualValue)
    {
        var json = await TestJson.Element(new { prop = actualValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is not equal to the expected value [{string.Join(",", expectedValue)}].",
                "The checked struct:",
                $"\t[[{string.Join(",", actualValue)}]]");
    }

    [Fact]
    public async Task HasArrayValueEquivalentToFailingWhenPropertyIsWrongKind()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValueEquivalentTo(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not an array.",
                "The checked struct:",
                "\t[42]");
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 1 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 1, 1 })]
    public async Task HasArrayValueEquivalentToCanBeNegate(int[] expectedValue, IEnumerable<int> actualValue)
    {
        var json = await TestJson.Element(new { prop = actualValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValueEquivalentTo(expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 1, 2 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task HasArrayValueEquivalentToNegationFailingWhenPropertyIsOfSpecifiedKind(int[] value, IEnumerable<int> equivalentValue)
    {
        var json = await TestJson.Element(new { prop = value });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValueEquivalentTo(equivalentValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is equivalent to [{string.Join(",", equivalentValue)}] whereas it must not.",
                "The checked struct:",
                $"\t[[{string.Join(",", value)}]]");
    }
}