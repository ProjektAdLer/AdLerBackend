using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetLMSToken;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Moodle.GetMoodleToken;

public class GerMoodleUserTokenHandlerTest
{
    private ILMS _ilmsMock;
    private GetLMSUserTokenHandler _systemUnderTest;

    [SetUp]
    public void SetUp()
    {
        _ilmsMock = Substitute.For<ILMS>();
        _systemUnderTest = new GetLMSUserTokenHandler(_ilmsMock);
    }

    [Test]
    public async Task Handle_Should_Return_Token()
    {
        // Arrange
        var request = new GetLMSTokenCommand
        {
            UserName = "username",
            Password = "password"
        };
        _ilmsMock.GetLMSUserTokenAsync(request.UserName, request.Password).Returns(new LMSUserTokenResponse
        {
            LMSToken = "token"
        });

        // Act
        var result = await _systemUnderTest.Handle(request, CancellationToken.None);

        // Assert
        await _ilmsMock.Received(1).GetLMSUserTokenAsync(request.UserName, request.Password);
        Assert.That("token", Is.EqualTo(result.LMSToken));
    }
}