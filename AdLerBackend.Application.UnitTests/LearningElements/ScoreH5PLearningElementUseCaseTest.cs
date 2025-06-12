using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Configuration;
using AutoBogus;
using Microsoft.Extensions.Options;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreH5PLearningElementUseCaseTest
{
    private ILMS _ilms;
    private ISerialization _serialization;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
        _serialization = Substitute.For<ISerialization>();
    }

    [TestCase("https://whatever.com")]
    [TestCase("https://whatever.com/")]
    public async Task ScoreH5PElement_Valid_CallsWebservices(string url)
    {
        // Arrange
        var configuration = Options.Create(new BackendConfig
        {
            MoodleUrl = url
        });

        _ilms.GetLMSUserDataAsync(Arg.Any<string>()).Returns(new LMSUserDataResponse
        {
            UserEmail = "email",
            UserId = 1,
            LMSUserName = "moodleUserName"
        });

        var fakeXapi = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXapi);

        _ilms.ScoreGenericElementViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(true);


        var systemUnderTest =
            new ScoreH5PElementStrategyHandler(_serialization, _ilms, configuration);

        // Act
        await systemUnderTest.Handle(new ScoreH5PElementStrategyCommand
        {
            ScoreElementParams = new ScoreElementParams
            {
                SerializedXapiEvent = "xapiEvent"
            },
            LmsModule = new LmsModule
            {
                contextid = 123,
                Id = 123,
                Name = "name"
            },

            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _ilms.Received(1).GetLMSUserDataAsync(Arg.Any<string>());
    }

    [Test]
    public async Task ScoreH5PElement_NoURLSet_ThrowsException()
    {
        // Arrange
        var configuration = Options.Create(new BackendConfig());

        var systemUnderTest = new ScoreH5PElementStrategyHandler(_serialization, _ilms, configuration);


        // Act
        // Assert
        Assert.ThrowsAsync<InvalidOperationException>(async () => await systemUnderTest.Handle(
            new ScoreH5PElementStrategyCommand
            {
                ScoreElementParams = new ScoreElementParams
                {
                    SerializedXapiEvent = "xapiEvent"
                },
                LmsModule = new LmsModule
                {
                    contextid = 123,
                    Id = 123,
                    Name = "name"
                },

                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}