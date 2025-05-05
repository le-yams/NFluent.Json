using System.Text.Json;
using NFluent.Json.Exceptions;
using NFluent.Json.Extensions;
using Xunit;

namespace NFluent.Json.Tests;

public class ExtensionGetRequiredElementAtShould
{
    [Theory]
    [InlineData("$.a.b", "$.a.b.c")]
    [InlineData(".a.b", ".a.b.c")]
    [InlineData("a.b", "a.b.c")]
    public async Task ReturnJsonPathTargetElementWithinObject(string pathB, string pathC)
    {
        // Arrange
        var json = await TestJson.Element(new
        {
            a = new
            {
                b = new
                {
                    c = 1
                }
            }
        });

        // Act
        var b = json.GetRequiredElementAt(pathB);
        var c = json.GetRequiredElementAt(pathC);

        // Assert
        Check.That(b).HasIntProperty("c", 1);
        Check.That(c).HasIntValue(1);
    }

    [Theory]
    [InlineData("$[0].a.b", "$[0].a.b.c")]
    [InlineData("[0].a.b", "[0].a.b.c")]
    public async Task ReturnJsonPathTargetElementWithinArray(string pathB, string pathC)
    {
        // Arrange
        var json = await TestJson.Element(new object[]
        {
            new
            {
                a = new
                {
                    b = new
                    {
                        c = 1
                    }
                }
            }
        });

        // Act
        var b = json.GetRequiredElementAt(pathB);
        var c = json.GetRequiredElementAt(pathC);

        // Assert
        Check.That(b).HasIntProperty("c", 1);
        Check.That(c).HasIntValue(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("$.a.")]
    [InlineData("$.a[boom]")]
    public async Task FailWithInvalidJsonPathQuery(string invalidQuery)
    {
        // Arrange
        var json = await TestJson.Element(new { });

        // Act & Assert
        Assert.Throws<InvalidPathException>(() => json.GetRequiredElementAt(invalidQuery));
    }

    [Fact]
    public async Task FailWhenNoElementFound()
    {
        // Arrange
        var json = await TestJson.Element(new { });

        // Act & Assert
        var thrown = Assert.Throws<JsonException>(() => json.GetRequiredElementAt("$.notExistingPath"));

        // Assert
        Check.That(thrown.Message).Contains("No element found at '$.notExistingPath'.");
    }

    [Fact]
    public async Task FailWhenMoreThanOneElementFound()
    {
        // Arrange
        var json = await TestJson.Element(new
        {
            array = new[]
            {
                new { a = 1 },
                new { a = 2 }
            }
        });

        // Act & Assert
        var thrown = Assert.Throws<JsonException>(() => json.GetRequiredElementAt("$.array[*].a"));

        // Assert
        Check.That(thrown.Message).Contains("Found more than one element at '$.array[*].a'.");
    }
}