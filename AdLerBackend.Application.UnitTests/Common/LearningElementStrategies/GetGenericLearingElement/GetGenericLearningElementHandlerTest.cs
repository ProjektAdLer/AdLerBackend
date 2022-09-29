using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using FluentAssertions;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.LearningElementStrategies.GetGenericLearingElement;

public class GetGenericLearningElementHandlerTest
{
    private IMoodle _moodle;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task ScoreGenericLearningElementStrategyHandler_Valid(bool expected)
    {
        // Arrange
        var systemUnderTest = new ScoreGenericLearningElementStrategyHandler(_moodle);
        _moodle.ScoreGenericLearningElement(Arg.Any<string>(), Arg.Any<int>()).Returns(expected);

        // Act
        var result = await systemUnderTest.Handle(new ScoreGenericLearningElementStrategyCommand
            {
                Module = new Modules
                {
                    Id = 1
                },
                WebServiceToken = "token"
            },
            CancellationToken.None);

        // Assert
        result.isSuceess.Should().Be(expected);
    }
}