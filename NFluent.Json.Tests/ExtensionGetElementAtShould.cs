using System.Text.Json;
using NFluent.Json.Exceptions;
using NFluent.Json.Extensions;
using Xunit;

namespace NFluent.Json.Tests;

public class ExtensionGetElementAtShould
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
        var b = json.GetElementAt(pathB);
        var c = json.GetElementAt(pathC);

        // Assert
        Check.That(b!.Value).HasIntProperty("c", 1);
        Check.That(c!.Value).HasIntValue(1);
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
        var b = json.GetElementAt(pathB);
        var c = json.GetElementAt(pathC);

        // Assert
        Check.That(b!.Value).HasIntProperty("c", 1);
        Check.That(c!.Value).HasIntValue(1);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("$.a.")]
    [InlineData("$.a[boom]")]
    public async Task FailWithInvalidJsonPath(string invalidPath)
    {
        // Arrange
        var json = await TestJson.Element(new { });

        // Act & Assert
        Assert.Throws<InvalidPathException>(() => json.GetElementAt(invalidPath));
    }

    [Fact]
    public async Task ReturnNullWhenNoElementFound()
    {
        // Arrange
        var json = await TestJson.Element(new { });

        // Act & Assert
        var result = json.GetElementAt("$.notExistingPath");

        // Assert
        Check.That(result).IsNull();
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
        var thrown = Assert.Throws<JsonException>(() => json.GetElementAt("$.array[*].a"));

        // Assert
        Check.That(thrown.Message).Contains("Found more than one element at '$.array[*].a'.");
    }
}