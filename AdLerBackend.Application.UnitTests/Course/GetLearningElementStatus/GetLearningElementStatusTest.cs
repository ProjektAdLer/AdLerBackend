using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllLearningElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Course.GetLearningElementStatus;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Course.GetLearningElementStatus;

public class GetLearningElementStatusTest
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
    public async Task GetLearningElementStatusHandler_Valid_GivesAllScores()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementStatusHandler(_mediator, _moodle);

        _mediator.Send(Arg.Any<GetAllLearningElementsFromLmsCommand>()).Returns(
            new GetAllLearningElementsFromLmsResponse
            {
                ModulesWithID = new List<ModuleWithId>
                {
                    new()
                    {
                        Id = 1,
                        Module = new Modules
                        {
                            contextid = 1,
                            Id = 1,
                            Instance = 1,
                            Name = "Name",
                            ModName = "h5pactivity"
                        }
                    }
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
        var result = await systemUnderTest.Handle(new GetLearningElementStatusCommand
        {
            CourseId = 1
        }, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, result.LearningElements.Count);
    }
}