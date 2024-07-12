using NFluent.Helpers;
using Xunit;

namespace NFluent.Json.Tests;

public class JsonElementNullValueCheckShould
{
    [Fact]
    public async Task HasNullValueWorks()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check
            .That(json.GetProperty("nullProp"))
            .HasNullValue();
    }

    [Fact]
    public async Task HasNullValueFailingWhenPropertyIsWrongKing()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check.ThatCode(() => Check.That(json.GetProperty("prop")).HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is not null.",
                "The checked struct:",
                "\t[42]");
    }

    [Fact]
    public async Task HasNullValueCanBeNegate()
    {
        var json = await TestJson.Element(new { prop = "42" });

        Check
            .That(json.GetProperty("prop"))
            .Not.HasNullValue();
    }

    [Fact]
    public async Task HasNullValueNegationFailingWhenPropertyIsOfSpecifiedKing()
    {
        var json = await TestJson.Element(new { nullProp = (string)null! });

        Check.ThatCode(() => Check.That(json.GetProperty("nullProp")).Not.HasNullValue())
            .IsAFailingCheckWithMessage(
                "",
                "The property value is null whereas it must not.",
                "The checked struct:",
                "\t[]");
    }
}