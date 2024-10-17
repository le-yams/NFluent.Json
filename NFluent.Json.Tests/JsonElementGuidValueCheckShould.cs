using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementGuidValueCheckShould
{
    [Fact]
    public async Task HasGuidValueWorks()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasGuidValue(expectedValue);
    }

    [Fact]
    public async Task HasGuidValueFailingWhenPropertyIsNotExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = Guid.NewGuid() });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is not equal to the expected value '{expectedValue}'.",
                "The checked struct:",
                $"\t[{json.GetProperty("prop").GetGuid()}]");
    }

    [Fact]
    public async Task HasGuidValueFailingWhenPropertyIsWrongKind()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = 42 });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not a Guid.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasGuidValueCanBeNegate()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = Guid.NewGuid() });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasGuidValue(expectedValue);
    }

    [Fact]
    public async Task HasIntValueNegationFailingWhenPropertyIsOfSpecifiedKind()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The property value is equal to '{expectedValue}' whereas it must not.",
                "The checked struct:",
                $"\t[{expectedValue}]");
    }
}