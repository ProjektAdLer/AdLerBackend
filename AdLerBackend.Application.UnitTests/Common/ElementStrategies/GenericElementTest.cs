using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GenericGetElementScoreStrategy;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using FluentAssertions;

namespace AdLerBackend.Application.UnitTests.Common.ElementStrategies;

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
        var result = await systemUnderTest.Handle(new GenericGetElementScoreScoreStrategyCommand
        {
            ElementId = 1,
            ElementMoule = new Modules
            {
                CompletionData = new CompletionData
                {
                    State = status
                }
            }
        }, CancellationToken.None);

        // Assert
        result.Success.Should().Be(expected);
    }
}