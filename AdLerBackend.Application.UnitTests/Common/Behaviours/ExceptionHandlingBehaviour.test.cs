using AdLerBackend.Application.Common.Behaviours;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetLMSToken;
using AdLerBackend.Application.LMS.GetUserData;
using MediatR;
using MediatR.Pipeline;

namespace AdLerBackend.Application.UnitTests.Common.Behaviours;

public class ExceptionHandlingBehaviourTest
{
    [Test]
    public Task ExceptionBehaviour_ValidPath_ShouldCreateTokenException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<LMSUserDataResponse>, LMSUserDataResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidTokenException>(() =>
            systemUnderTest.Handle(new GetLMSUserDataCommand(), new LmsException
                {
                    LmsErrorCode = "invalidtoken"
                },
                new RequestExceptionHandlerState<LMSUserDataResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task ExceptionBehaviour_ValidPath_ShouldCreateLoginException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<LMSUserTokenResponse>, LMSUserTokenResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<InvalidLmsLoginException>(() =>
            systemUnderTest.Handle(new GetLMSTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidlogin"
                },
                new RequestExceptionHandlerState<LMSUserTokenResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }

    [Test]
    public Task ExceptionBehaviour_Invalid_ShourldReturnInputException()
    {
        // Arrange
        var systemUnderTest =
            new ExceptionHandlingBehaviour<IRequest<LMSUserTokenResponse>, LMSUserTokenResponse, LmsException>();

        // Act
        Assert.ThrowsAsync<LmsException>(() =>
            systemUnderTest.Handle(new GetLMSTokenCommand(), new LmsException
                {
                    LmsErrorCode = "invalidErrorCode"
                },
                new RequestExceptionHandlerState<LMSUserTokenResponse>(), CancellationToken.None));
        return Task.CompletedTask;
    }

    // [Test]
    // public MoodleTaskResponse ExceptionBehaviour_NotLmsException_ForwardException()
    // {
    //     // Arrange
    //     var systemUnderTest =
    //         new ExceptionHandlingBehaviour<IRequest<MoodleUserTokenResponse>, MoodleUserTokenResponse, LmsException>();
    //
    //     // Act
    //     Assert.ThrowsAsync<LmsException>(() =>
    //         systemUnderTest.Handle(new GetMoodleTokenCommand(), new Exception("test"),
    //             new RequestExceptionHandlerState<MoodleUserTokenResponse>(), CancellationToken.None));
    //     return MoodleTaskResponse.CompletedTask;
    // }
}