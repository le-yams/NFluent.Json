using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementArrayPropertyCheckShould
{
    [Fact]
    public async Task HasArrayPropertyWorksOnHavingPropertyWithExpectedNumbers()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasArrayPropertyWorksOnHavingPropertyWithExpectedStrings()
    {
        var expectedValue = new[] { "1", "2" };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasArrayPropertyWorksOnHavingPropertyWithExpectedBooleans()
    {
        var expectedValue = new[] { true, false, true };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasArrayPropertyWorksOnHavingPropertyWithExpectedObjects()
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
    public async Task HasArrayPropertyWorksOnHavingPropertyWithExpectedNullValues()
    {
        var expectedValue = new int?[] { 1, null };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.That(json).HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasArrayPropertyFailingWhenPropertyIsUndefined()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propB", new[] { 1, 2 })).IsAFailingCheckWithMessage(
            "",
            "The 'propB' property is undefined.",
            "The checked struct:",
            "\t[{\"propA\":\"\"}]");
    }

    [Fact]
    public async Task HasArrayPropertyFailingWhenPropertyIsNotAnArray()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", new[] { 1, 2 })).IsAFailingCheckWithMessage(
            "",
            "The 'propA' property kind is not array.",
            "The checked struct:",
            "\t[{\"propA\":\"42\"}]");
    }

    [Fact]
    public async Task HasArrayPropertyFailingWhenExpectingArrayHasDifferentLength()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue.Concat(new[] { 3 }) });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property value is not equal to the expected value [1,2].",
            "The checked struct:",
            "\t[{\"propA\":[1,2,3]}]");
    }

    [Fact]
    public async Task HasArrayPropertyFailingWhenExpectingArrayHasSameLengthButDifferentValues()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue.Reverse() });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property value is not equal to the expected value [1,2].",
            "The checked struct:",
            "\t[{\"propA\":[2,1]}]");
    }

    [Fact]
    public async Task HasArrayPropertyFailingWhenExpectingArrayHasSameLengthButDifferentObjectValues()
    {
        var expectedValue = new[]
        {
            new { id = 1, name = "foo" },
            new { id = 2, name = "bar" }
        };
        var json = await TestJson.Element(new { propA = expectedValue.Reverse() });

        Check.ThatCode(() => Check.That(json).HasArrayProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property value is not equal to the expected value [{\"id\":1,\"name\":\"foo\"},{\"id\":2,\"name\":\"bar\"}].",
            "The checked struct:",
            "\t[{\"propA\":[{\"id\":2,\"name\":\"bar\"},{\"id\":1,\"name\":\"foo\"}]}]");
    }

    [Fact]
    public async Task HasArrayPropertyCanBeNegateWithUndefinedProperty()
    {
        var json = await TestJson.Element(new { propA = "" });

        Check.That(json).Not.HasArrayProperty("propB", new[] { 1, 2 });
    }

    [Fact]
    public async Task HasArrayPropertyCanBeNegateWithWrongPropertyKind()
    {
        var json = await TestJson.Element(new { propA = "42" });

        Check.That(json).Not.HasArrayProperty("propA", new[] { 1, 2 });
    }

    [Fact]
    public async Task HasArrayPropertyCanBeNegateWithWrongLength()
    {
        var expectedValue = new[] { 1, 2 };
        var notExpectedValue = expectedValue.Concat(new[] { 3 });
        var json = await TestJson.Element(new { propA = notExpectedValue });

        Check.That(json).Not.HasArrayProperty("propA", expectedValue);
    }

    [Fact]
    public async Task HasArrayPropertyNegationFailingWhenHavingThePropertyWithExpectedValue()
    {
        var expectedValue = new[] { 1, 2 };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasArrayProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property 'propA' is present and has value [1,2] whereas it must not.",
            "The checked struct:",
            "\t[{\"propA\":[1,2]}]");
    }

    [Fact]
    public async Task HasArrayPropertyNegationFailingWhenHavingThePropertyWithExpectedObjectsValue()
    {
        var expectedValue = new[] { new{id=1}, new{id=2} };
        var json = await TestJson.Element(new { propA = expectedValue });

        Check.ThatCode(() => Check.That(json).Not.HasArrayProperty("propA", expectedValue)).IsAFailingCheckWithMessage(
            "",
            "The property 'propA' is present and has value [{\"id\":1},{\"id\":2}] whereas it must not.",
            "The checked struct:",
            "\t[{\"propA\":[{\"id\":1},{\"id\":2}]}]");
    }
}