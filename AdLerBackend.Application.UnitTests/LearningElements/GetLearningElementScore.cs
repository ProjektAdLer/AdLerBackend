using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class GetLearningElementScore
{
    private IMediator _mediator;
    private IMoodle _moodle;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    public async Task GetLearningElementScore_Valid_GetsScoreFromApi()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementScoreHandler(_mediator, _moodle);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    contextid = 1,
                    Id = 1,
                    Instance = 1,
                    Name = "name",
                    ModName = "h5pactivity"
                }
            });

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

        // Act
        var result = await systemUnderTest.Handle(new GetLearningElementScoreCommand
        {
            learningElementId = 1,
            lerningWorldId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        await _moodle.Received(1).GetH5PAttemptsAsync("token", 1);
    }

    [Test]
    public async Task GetLearningElementScore_NotH5P_Throws()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementScoreHandler(_mediator, _moodle);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    contextid = 1,
                    Id = 1,
                    Instance = 1,
                    Name = "name",
                    ModName = "Noh5pactivity"
                }
            });

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

        // Act
        // Assert
        Assert.ThrowsAsync<Exception>(async () => await systemUnderTest.Handle(
            new GetLearningElementScoreCommand
            {
                learningElementId = 1,
                lerningWorldId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}