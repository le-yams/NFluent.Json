using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayValueCheckShould
{
    [Fact]
    public async Task PassWithBooleans()
    {
        var expectedValue = new[] { true, false, true };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWithObjects()
    {
        var expectedValue = new[] { new { id = 1, name = "foo" }, new { id = 2, name = "bar" } };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWithNullValues()
    {
        var expectedValue = new[] { "1", null, "2" };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWithNumbers()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWithStrings()
    {
        var expectedValue = new[] { "1", "2" };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongPropertyKind()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = "42" });

        Check.That(json.GetProperty("prop")).Not.HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongSize()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue.Concat(new[] { 3 }) });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValue(expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWhenNegatedWithDifferentValues(int[] expectedValue, IEnumerable<int> notEquivalentValue)
    {
        var json = await TestJson.Element(new { prop = notEquivalentValue });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValue(expectedValue);
    }

    [Theory]
    [InlineData(new[] { 1, 2 }, new[] { 3, 4 })]
    [InlineData(new[] { 1, 2 }, new[] { 2, 1 })]
    public async Task PassWhenNegatedWithDifferentObjectValues(int[] expectedIds, IEnumerable<int> notEquivalentIds)
    {
        var value = expectedIds.Select(i => new { id = i }).ToArray();
        var notEquivalentValue = notEquivalentIds.Select(i => new { id = i }).ToArray();

        var json = await TestJson.Element(new { prop = value });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValue(notEquivalentValue);
    }

    [Fact]
    public async Task FailWhenElementIsNotAnArray()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("propA")).HasArrayValue(new[] { 1, 2 }))
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

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[[1,2,3]]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = new[] { 3, 4 } });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[[3,4]]");
    }

    [Fact]
    public async Task FailWhenArrayHasDifferentObjectValues()
    {
        var expectedValue = new[]
        {
            new { id = 1, name = "foo" },
            new { id = 2, name = "bar" }
        };
        var json = await TestJson.Element(new { prop = expectedValue.Reverse() });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not equal to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
                "The checked struct:",
                "\t[[{\"id\":2,\"name\":\"bar\"},{\"id\":1,\"name\":\"foo\"}]]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to [1,2] whereas it must not.",
                "The checked struct:",
                "\t[[1,2]]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedObjectsValue()
    {
        var expectedValue = new[] { new { id = 1 }, new { id = 2 } };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is equal to [{\"id\":1},{\"id\":2}] whereas it must not.",
                "The checked struct:",
                "\t[[{\"id\":1},{\"id\":2}]]");
    }
}