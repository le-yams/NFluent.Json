using System.Net;
using System.Net.Http.Json;
using NFluent.Json.Extensions;
using Xunit;

namespace NFluent.Json.Tests;

public class ExtensionHttpContentReadJsonRootElementShould
{
    [Fact]
    public async Task ReadJsonElement()
    {
        // Arrange
        var httpContent = JsonContent.Create(new
        {
            a = 1
        });

        // Act
        var json = await httpContent.ReadJsonRootElementAsync();

        // Assert
        Check.That(json).HasIntProperty("a", 1);
    }

    [Fact]
    public async Task FailWhenContentIsNull()
    {
        // Arrange
        HttpContent? httpContent = null;

        // Act
        var thrown = await Assert.ThrowsAsync<ArgumentException>(() => httpContent.ReadJsonRootElementAsync());

        // Assert
        Check.That(thrown.Message).IsEqualTo("http content is null");
    }

    [Fact]
    public async Task FailWhenWhenContentIsEmpty()
    {
        // Arrange
        var httpContent = new StringContent(string.Empty);

        // Act
        var thrown = await Assert.ThrowsAsync<ArgumentException>(() => httpContent.ReadJsonRootElementAsync());

        // Assert
        Check.That(thrown.Message).IsEqualTo("http content is empty");
    }
}