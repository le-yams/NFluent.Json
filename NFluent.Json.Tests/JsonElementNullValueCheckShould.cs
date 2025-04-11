using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementNullValueCheckShould
{
    [Fact]
    public async Task PassWithNullValue()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check
            .That(json.GetProperty("nullProp"))
            .HasNullValue();
    }

    [Fact]
    public async Task PassWhenNegatedWithNullValue()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasNullValue();
    }

    [Fact]
    public async Task FailWhenNotANullValue()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is not null.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task FailWhenNegatedWithNullValue()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check.ThatCode(() => Check.That(json.GetProperty("nullProp")).Not.HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The element is null whereas it must not.",
                "The checked struct:",
                "\t[]");
    }
}