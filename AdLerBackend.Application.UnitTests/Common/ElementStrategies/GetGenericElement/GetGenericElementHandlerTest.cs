using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using FluentAssertions;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Common.ElementStrategies.GetGenericElement;

public class GetGenericElementHandlerTest
{
    private ILMS _ilms;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task ScoreGenericLearningElementStrategyHandler_Valid(bool expected)
    {
        // Arrange
        var systemUnderTest = new ScoreGenericElementStrategyHandler(_ilms);
        _ilms.ScoreGenericElementViaPlugin(Arg.Any<string>(), Arg.Any<int>()).Returns(expected);

        // Act
        var result = await systemUnderTest.Handle(new ScoreGenericElementStrategyCommand
            {
                Module = new Modules
                {
                    Id = 1
                },
                WebServiceToken = "token"
            },
            CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(expected);
    }
}