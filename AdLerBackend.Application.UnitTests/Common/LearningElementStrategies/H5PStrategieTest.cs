using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.LearningElementStrategies.GetLearningElementScoreStrategies.
    GetH5PLearningElementScoreStrategy;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Common.LearningElementStrategies;

public class H5PStrategieTest
{
    private IMoodle _moodle;

    [SetUp]
    public void Setup()
    {
        _moodle = Substitute.For<IMoodle>();
    }

    [Test]
    public async Task Handle_Valid()
    {
        // Arrange
        var systemUnderTest = new GetH5PLearningElementScoreStrategyHandler(_moodle);
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
        var result = await systemUnderTest.Handle(new GetH5PLearningElementScoreStrategyCommand
        {
            ElementId = 1,
            LearningElementMoule = new Modules
            {
                Instance = 1
            },
            WebServiceToken = "token"
        }, CancellationToken.None);
    }
}