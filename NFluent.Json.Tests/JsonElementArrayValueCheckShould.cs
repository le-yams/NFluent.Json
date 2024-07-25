using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayValueCheckShould
{
    [Fact]
    public async Task HasArrayValueWorksWithNumbers()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueWorksWithStrings()
    {
        var expectedValue = new[] { "1", "2" };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueWorksWithBooleans()
    {
        var expectedValue = new[] { true, false, true };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueWorksWithNullValues()
    {
        var expectedValue = new[] { "1", null, "2" };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueWorksWithObjects()
    {
        var expectedValue = new[] { new { id = 1, name = "foo" }, new { id = 2, name = "bar" } };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueFailingWhenPropertyIsArrayWithDifferentLength()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[[1,2,3]]");
    }

    [Fact]
    public async Task HasArrayValueFailingWhenPropertyIsArrayWithSameLengthButDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = new[] { 3, 4 } });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not equal to the expected value [1,2].",
                "The checked struct:",
                "\t[[3,4]]");
    }

    [Fact]
    public async Task HasArrayValueFailingWhenPropertyIsArrayWithSameLengthButDifferentObjectValues()
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
                "The property value is not equal to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
                "The checked struct:",
                "\t[[{\"id\":2,\"name\":\"bar\"},{\"id\":1,\"name\":\"foo\"}]]");
    }

    [Fact]
    public async Task HasArrayValueFailingWhenPropertyIsWrongKind()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not an array.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasArrayValueCanBeNegate()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue.Concat(new[] { 3 }) });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasArrayValue(expectedValue);
    }

    [Fact]
    public async Task HasArrayValueNegationFailingWithSameValue()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to [1,2] whereas it must not.",
                "The checked struct:",
                "\t[[1,2]]");
    }

    [Fact]
    public async Task HasArrayValueNegationFailingWithSameObjectsValue()
    {
        var expectedValue = new[] { new { id = 1 }, new { id = 2 } };
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasArrayValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is equal to [{\"id\":1},{\"id\":2}] whereas it must not.",
                "The checked struct:",
                "\t[[{\"id\":1},{\"id\":2}]]");
    }
}