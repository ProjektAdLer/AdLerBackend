using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Infrastructure.Services;

namespace AdLerBackend.Infrastructure.UnitTests.Services;

public class AtfSerializationTest
{
    [Test]
    public Task CanSerializeAtf()
    {
        // Arrange
        var systemUnderTest = new SerializationService();

        // Act
        var readAllText = File.ReadAllText(
            "D:\\Projects\\AdLer\\TransferFileFormat\\Adaptivity - WIP V2\\AdLerWorldTransferXXV2 copy.json");

        var worldAtfResponse = systemUnderTest.GetObjectFromJsonString<WorldAtfResponse>(
            readAllText);

        var serializeObject = systemUnderTest.ClassToJsonString(worldAtfResponse);

        // Assert
        Assert.That(true);
        return Task.CompletedTask;
    }
}