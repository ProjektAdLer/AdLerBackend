using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LMS.GetUserData;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Moodle.GetUserData;

public class GetMoodleUserDataHandlerTest
{
    private ILMS _ilmsMock;
    private GetLMSUserDataHandler _systemUnderTest;

    [SetUp]
    public void SetUp()
    {
        _ilmsMock = Substitute.For<ILMS>();
        _systemUnderTest = new GetLMSUserDataHandler(_ilmsMock);
    }

    [Test]
    public async Task Handle_ValidResponse_CallsServiceAndReturns()
    {
        // Arrange
        var request = new GetLMSUserDataCommand
        {
            WebServiceToken = "testToken"
        };
        _ilmsMock.GetLMSUserDataAsync(request.WebServiceToken).Returns(new LMSUserDataResponse
        {
            IsAdmin = true,
            LMSUserName = "TestNutzer"
        });
        // Act
        var result = await _systemUnderTest.Handle(request, new CancellationToken());

        // Assert
        await _ilmsMock.Received(1).GetLMSUserDataAsync(request.WebServiceToken);
        Assert.That(result.IsAdmin, Is.True);
        Assert.That(result.LMSUserName, Is.EqualTo("TestNutzer"));
    }
}