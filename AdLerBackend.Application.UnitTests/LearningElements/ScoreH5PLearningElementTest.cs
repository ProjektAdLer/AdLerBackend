using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AutoBogus;
using Microsoft.Extensions.Configuration;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreH5PLearningElementTest
{
    private IMoodle _moodle;
    private ISerialization _serialization;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _serialization = Substitute.For<ISerialization>();
    }

    [Test]
    public async Task ScoreH5PElement_Valid_CallsWebservices()
    {
        // Arrange

        var inMemorySettings = new Dictionary<string, string>
        {
            {"moodleUrl", "https://whatever.com"}
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        var fakeXapi = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXapi);

        _moodle.GetH5PAttemptsAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new H5PAttempts
        {
            usersattempts = new List<Usersattempt>
            {
                new()
                {
                    scored = new Scored
                    {
                        attempts = new List<Attempt>
                        {
                            new()
                            {
                                success = 1
                            }
                        }
                    }
                }
            }
        });


        var systemUnderTest =
            new ScoreH5PElementStrategyHandler(_serialization, _moodle, configuration);

        // Act
        await systemUnderTest.Handle(new ScoreH5PElementStrategyCommand
        {
            ScoreElementParams = new ScoreElementParams
            {
                SerializedXapiEvent = "xapiEvent"
            },
            Module = new Modules
            {
                contextid = 123,
                Id = 123,
                Instance = 123,
                Name = "name"
            },

            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _moodle.Received(1).GetMoodleUserDataAsync(Arg.Any<string>());
    }
}