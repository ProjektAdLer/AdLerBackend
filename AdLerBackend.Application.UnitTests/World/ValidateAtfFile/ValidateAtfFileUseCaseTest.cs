using System.Text;
using AdLerBackend.Application.World.ValidateATFFile;
using FluentAssertions;
using FluentValidation;
using MediatR;

namespace AdLerBackend.Application.UnitTests.World.ValidateAtfFile;

public class ValidateAtfFileUseCaseTest
{
    private ValidateAtfFileUseCase _systemUnderTest;

    [SetUp]
    public void Setup()
    {
        _systemUnderTest = new ValidateAtfFileUseCase();
    }

    [Test]
    public async Task Handle_ValidJson_ReturnsUnit()
    {
        // Arrange
        var json =
            "{\"fileVersion\":\"0.4\",\"amgVersion\":\"1.0\",\"author\":\"Peter\",\"language\":\"de\",\"world\":{\"worldName\":\"Name der Welt\",\"worldUUID\":\"12345678-1234-1234-1234-123456789012\",\"worldDescription\":\"Beschreibung der Welt\",\"worldGoals\":[\"Ziel 1\",\"Ziel 2\"],\"topics\":[{\"topicId\":1,\"topicName\":\"A\",\"topicContents\":[1,2]}],\"spaces\":[{\"spaceId\":1,\"spaceUUID\":\"12345678-1234-1234-1234-123456789012\",\"spaceName\":\"Name des Lernraums\",\"spaceDescription\":\"Beschreibung des Lernraums\",\"spaceGoals\":[\"Ziel 1\",\"Ziel 2\"],\"spaceSlotContents\":[1,null,2,3,null,null],\"requiredPointsToComplete\":100,\"requiredSpacesToEnter\":\"(5)v((7)^(4))\",\"spaceTemplate\":\"rectangle2x3\",\"spaceTemplateStyle\":\"casino\"}],\"elements\":[{\"elementId\":1,\"elementUUID\":\"12345678-1234-1234-1234-123456789012\",\"elementName\":\"Name des Elements\",\"elementDescription\":\"Beschreibung des Lernelements\",\"elementGoals\":[\"Ziel 1\",\"Ziel 2\"],\"elementCategory\":\"image\",\"elementFileType\":\"image:png\",\"elementMaxScore\":123,\"elementModel\":\"ArcadeMachine\"}]}}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

        var command = new ValidateATFFileCommand
        {
            ATFFileStream = stream
        };

        // Act
        var result = await _systemUnderTest.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(Unit.Value);
    }

    [Test]
    public void Handle_InvalidJson_ThrowsValidationException()
    {
        // Arrange
        var invalidJson = @"{ ""foo"": ""bar"" }{""baz"":""qux""}";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(invalidJson));

        var command = new ValidateATFFileCommand
        {
            ATFFileStream = stream
        };

        // Act & Assert
        Assert.ThrowsAsync<ValidationException>(async () =>
            await _systemUnderTest.Handle(command, CancellationToken.None));
    }
}