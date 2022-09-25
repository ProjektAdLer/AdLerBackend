using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.H5P.ScoreH5PElement;
using AutoBogus;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreLearningElementTest
{
    private IMediator _mediator;
    private IMoodle _moodle;
    private ISerialization _serialization;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _moodle = Substitute.For<IMoodle>();
        _serialization = Substitute.For<ISerialization>();
    }

    [Test]
    public async Task ScoreH5PElement_Valid_CallsWebservices()
    {
        // Arrange

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    contextid = 1234,
                    Id = 123,
                    Name = "fileName",
                    ModName = "ModName"
                }
            });

        var fakeXAPI = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXAPI);


        var systemUnderTest =
            new ScoreH5PElementHandler(_serialization, _moodle, _mediator);

        // Act
        await systemUnderTest.Handle(new ScoreElementCommand
        {
            CourseId = 1,
            ElementId = 1337,
            ScoreElementParams = new ScoreElementParams
            {
                SerializedXapiEvent = "xapiEvent"
            },
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _moodle.Received(1).GetMoodleUserDataAsync(Arg.Any<string>());
    }


    [Test]
    public async Task ScoreH5PElement_H5PFileNotExistent_Throws()
    {
        // Arrange

        _moodle.GetMoodleUserDataAsync(Arg.Any<string>()).Returns(new MoodleUserDataResponse
        {
            IsAdmin = false,
            UserEmail = "email",
            UserId = 1,
            MoodleUserName = "moodleUserName"
        });

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = null
            });

        var fakeXAPI = AutoFaker.Generate<RawH5PEvent>();

        _serialization.GetObjectFromJsonString<RawH5PEvent>(Arg.Any<string>()).Returns(fakeXAPI);


        var systemUnderTest =
            new ScoreH5PElementHandler(_serialization, _moodle, _mediator);

        // Act
        // Assert
        Assert.ThrowsAsync<NotFoundException>(async () =>
            await systemUnderTest.Handle(new ScoreElementCommand
            {
                CourseId = 1,
                ElementId = 1337,
                ScoreElementParams = new ScoreElementParams
                {
                    SerializedXapiEvent = "xapiEvent"
                },
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}