using AdLerBackend.API.Controllers.LMSUserService;
using AdLerBackend.Application.LMS.GetLMSToken;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.LMSUserService;

public class LmsLoginControllerTest
{
    // ANF-ID: [BPG20]
    // ANF-ID: [BPG7]
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