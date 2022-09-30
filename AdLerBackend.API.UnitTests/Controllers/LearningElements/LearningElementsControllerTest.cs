using AdLerBackend.API.Controllers.LearningElements;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
using AdLerBackend.Application.LearningElement.ScoreLearningElement;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.LearningElements;

public class LearningElementsControllerTest
{
    [Test]
    public async Task ScoreElement_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LearningElementsController(mediatorMock);

        // Act
        await controller.ScoreElement("token", 1, 2, new ScoreElementParams
        {
            SerializedXapiEvent = "foo"
        });

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<ScoreLearningElementCommand>());
    }

    [Test]
    public async Task GetElementScore_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LearningElementsController(mediatorMock);

        // Act
        await controller.GetElementScore("token", 1, 2);

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetLearningElementScoreCommand>());
    }
}