using AdLerBackend.API.Controllers.LMSUserService;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.LMS.GetLMSToken;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace AdLerBackend.API.UnitTests.Controllers.LMSUserService;

public class LMSLoginControllerTest
{
    [Test]
    public async Task GetUserData_ShouldForwardCallToMoodleLoginService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LmsLoginController(mediatorMock);

        // Act
        await controller.GetLmsUserData(new GetLMSUserDataCommand
        {
            WebServiceToken = "TestToken"
        });

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetLMSUserDataCommand>(x => x.WebServiceToken == "TestToken"));
    }

    [Test]
    public Task GetUserData_ReturnsBadRequest_WhenLoginFails()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        mediatorMock.Send(Arg.Any<GetLMSUserDataCommand>()).Throws(
            new InvalidLMSLoginException());


        var controller = new LmsLoginController(mediatorMock);

        // Expect exception to be thrown
        Assert.ThrowsAsync<InvalidLMSLoginException>(() => controller.GetLmsUserData(new GetLMSUserDataCommand
        {
            WebServiceToken = "TestToken"
        }));
        return Task.CompletedTask;
    }

    [Test]
    public async Task Login_ShouldForwardCallToMoodleService()
    {
        // Arrange
        var mediatorMock = Substitute.For<IMediator>();
        var controller = new LmsLoginController(mediatorMock);

        // Act
        await controller.GetLmsUserToken(new GetLMSTokenCommand
        {
            Password = "TestPassword",
            UserName = "TestUsername"
        });

        // Assert
        await mediatorMock.Received(1).Send(
            Arg.Is<GetLMSTokenCommand>(x => x.Password == "TestPassword" && x.UserName == "TestUsername"));
    }
}