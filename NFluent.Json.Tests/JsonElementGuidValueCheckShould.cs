using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementGuidValueCheckShould
{
    [Fact]
    public async Task PassWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = expectedValue });

        Check
            .That(json.GetProperty("prop"))
            .HasGuidValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = Guid.NewGuid() });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasGuidValue(expectedValue);
    }

    [Fact]
    public async Task PassWhenNegatedWithWrongKind()
    {
        var json = await TestJson.Element(new { prop = "not a guid" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasGuidValue(Guid.NewGuid());
    }

    [Fact]
    public async Task FailWithWrongValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = Guid.NewGuid() });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is not equal to the expected value '{expectedValue}'.",
                "The checked struct:",
                $"\t[{json.GetProperty("prop").GetGuid()}]");
    }

    [Fact]
    public async Task FailWhenNotAGuid()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = "not a guid" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                "The element is not a Guid.",
                "The checked struct:",
                "\t[not a guid]");
    }

    [Fact]
    public async Task FailWhenNegatedWithExpectedValue()
    {
        var expectedValue = Guid.NewGuid();
        var json = await TestJson.Element(new { prop = expectedValue });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).Not.HasGuidValue(expectedValue))
            .IsAFailingCheckWithMessage(
                "",
                $"The element is equal to '{expectedValue}' whereas it must not.",
                "The checked struct:",
                $"\t[{expectedValue}]");
    }
}