using AdLerBackend.API.Controllers.LearningElements.H5P;
using AdLerBackend.Application.LearningElement.H5P.GetH5PFilePath;
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
        var controller = new H5PController(mediatorMock);

        // Act
        await controller.GetH5PFilePath("token", 1, 2);

        // Assert
        await mediatorMock.Received(1).Send(Arg.Any<GetH5PFilePathCommand>());
    }
}