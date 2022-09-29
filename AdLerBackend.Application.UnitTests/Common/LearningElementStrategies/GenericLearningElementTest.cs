using AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GenericGetLearningElementScoreStrategy;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using FluentAssertions;

namespace AdLerBackend.Application.UnitTests.Common.LearningElementStrategies;

public class GenericLearningElementTest
{
    [TestCase(1, true)]
    [TestCase(0, false)]
    [TestCase(123456, false)]
    [TestCase(-123456, false)]
    public async Task Handle_Returns_SuccessStatus(int status, bool expected)
    {
        // Arrange
        var systemUnderTest = new GenericGetLearningElementScoreStrategyHandler();

        // Act
        var result = await systemUnderTest.Handle(new GenericGetLearningElementScoreScoreStrategyCommand
        {
            ElementId = 1,
            LearningElementMoule = new Modules
            {
                CompletionData = new CompletionData
                {
                    State = status
                }
            }
        }, CancellationToken.None);

        // Assert
        result.successss.Should().Be(expected);
    }
}