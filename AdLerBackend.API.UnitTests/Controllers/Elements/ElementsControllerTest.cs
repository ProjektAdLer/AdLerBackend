using AdLerBackend.API.Controllers.Elements;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Element.GetElementScore;
using AdLerBackend.Application.Element.ScoreElement;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Elements;

public class ElementsControllerTest
{
    [Test]
    public async Task ScoreElement_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new ElementsController(mediatorMock);

        // Act
        await controller.ScoreElement("token", 1, 2, new ScoreElementParams
        {
            SerializedXAPIEvent = "foo"
        });

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<ScoreElementCommand>());
    }

    [Test]
    public async Task GetElementScore_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new ElementsController(mediatorMock);

        // Act
        await controller.GetElementScore("token", 1, 2);

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetElementScoreCommand>());
    }
}