using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GetH5PElementScoreStrategy;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using NSubstitute;

#pragma warning disable CS8618

namespace AdLerBackend.Application.UnitTests.Common.ElementStrategies;

public class H5PStrategieTest
{
    private ILMS _ilms;

    [SetUp]
    public void Setup()
    {
        _ilms = Substitute.For<ILMS>();
    }

    [Test]
    public async Task Handle_Valid()
    {
        // Arrange
        var systemUnderTest = new GetH5PElementScoreStrategyHandler(_ilms);
        _ilms.GetH5PAttemptsAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(new H5PAttempts
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
        var result = await systemUnderTest.Handle(new GetH5PElementScoreStrategyCommand
        {
            ElementId = 1,
            ElementMoule = new Modules
            {
                Instance = 1
            },
            WebServiceToken = "token"
        }, CancellationToken.None);
    }
}