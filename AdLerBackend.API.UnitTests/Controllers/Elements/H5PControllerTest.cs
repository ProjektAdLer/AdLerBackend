using AdLerBackend.API.Controllers.Elements;
using AdLerBackend.Application.Element.GetElementSource;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Elements;

public class H5PControllerTest
{
    [Test]
    public async Task ScoreElement_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new ElementsController(mediatorMock);

        // Act
        await controller.GetElementSource("token", 1, 2);

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetElementSourceCommand>());
    }
}