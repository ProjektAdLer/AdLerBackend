using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AdLerBackend.API.UnitTests.Controllers.PlayerController;

public class PlayerControllerTests
{
    // ANF-ID: [BPG20]
    [Test]
    public async Task GetUserData_ShouldForwardCallToMoodleLoginService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new API.Controllers.Player.PlayerController(mediatorMock);

        // Act
        await controller.GetLmsUserData("TestToken");

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetLMSUserDataCommand>(x => x.WebServiceToken == "TestToken"));
    }

    // ANF-ID: [BPG20]
    [Test]
    public Task GetUserData_ReturnsBadRequest_WhenLoginFails()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        mediatorMock.Send(Arg.Any<GetLMSUserDataCommand>()).Throws(
            new InvalidLmsLoginException());


        var controller = new API.Controllers.Player.PlayerController(mediatorMock);

        // Expect exception to be thrown
        Assert.ThrowsAsync<InvalidLmsLoginException>(() => controller.GetLmsUserData("TestToken"));
        return Task.CompletedTask;
    }
}