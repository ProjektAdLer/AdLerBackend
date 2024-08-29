using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Common.Utils;

namespace AdLerBackend.Application.UnitTests.Common.Utils;

[TestFixture]
public class IdExtractorTests
{
    [Test]
    public void GetQuestionIdFromUuid_WithValidUuid_ReturnsCorrectQuestionId()
    {
        // Arrange
        const int expectedQuestionId = 1;
        var uuid = Guid.NewGuid();
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>
                {
                    new()
                    {
                        AdaptivityQuestions = new List<AdaptivityQuestion>
                        {
                            new() { QuestionId = expectedQuestionId, QuestionUuid = uuid }
                        }
                    }
                }
            }
        };

        // Act
        var result = IdExtractor.GetQuestionIdFromUuid(uuid, adaptivityElement);

        // Assert
        Assert.That(result, Is.EqualTo(expectedQuestionId));
    }

    [Test]
    public void GetQuestionIdFromUuid_WithInvalidUuid_ThrowsException()
    {
        // Arrange
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>()
            }
        };

        // Act & Assert
        Assert.Throws<Exception>(() => IdExtractor.GetQuestionIdFromUuid(Guid.NewGuid(), adaptivityElement));
    }

    [Test]
    public void GetTaskIdFromUuid_WithValidUuid_ReturnsCorrectTaskId()
    {
        // Arrange
        const int expectedTaskId = 1;
        var uuid = Guid.NewGuid();
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>
                {
                    new() { TaskId = expectedTaskId, TaskUuid = uuid }
                }
            }
        };

        // Act
        var result = IdExtractor.GetTaskIdFromUuid(uuid, adaptivityElement);

        // Assert
        Assert.That(result, Is.EqualTo(expectedTaskId));
    }

    [Test]
    public void GetTaskIdFromUuid_WithInvalidUuid_ThrowsException()
    {
        // Arrange
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>()
            }
        };

        // Act & Assert
        Assert.Throws<Exception>(() => IdExtractor.GetTaskIdFromUuid(Guid.NewGuid(), adaptivityElement));
    }

    [Test]
    public void GetUuidFromQuestionId_WithValidQuestionId_ReturnsCorrectUuid()
    {
        // Arrange
        var expectedUuid = Guid.NewGuid();
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>
                {
                    new()
                    {
                        AdaptivityQuestions = new List<AdaptivityQuestion>
                        {
                            new() { QuestionId = 1, QuestionUuid = expectedUuid }
                        }
                    }
                }
            }
        };

        // Act
        var result = IdExtractor.GetUuidFromQuestionId(1, adaptivityElement);

        // Assert
        Assert.That(result, Is.EqualTo(expectedUuid));
    }

    [Test]
    public void GetUuidFromQuestionId_WithInvalidQuestionId_ThrowsException()
    {
        // Arrange
        var adaptivityElement = new AdaptivityElement
        {
            AdaptivityContent = new AdaptivityContent
            {
                AdaptivityTasks = new List<AdaptivityTask>
                {
                    new()
                    {
                        AdaptivityQuestions = new List<AdaptivityQuestion>()
                    }
                }
            }
        };

        // Act & Assert
        Assert.Throws<Exception>(() => IdExtractor.GetUuidFromQuestionId(99, adaptivityElement));
    }
}