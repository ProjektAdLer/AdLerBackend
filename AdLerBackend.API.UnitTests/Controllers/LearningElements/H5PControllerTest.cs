using AdLerBackend.API.Controllers.LearningElements;
using AdLerBackend.Application.LearningElement.GetLearningElementSource;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.LearningElements;

public class H5PControllerTest
{
    [Test]
    public async Task ScoreElement_ForwardsCall()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LearningElementsController(mediatorMock);

        // Act
        await controller.GetLearningElementSource("token", 1, 2);

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetLearningElementSourceCommand>());
    }
}