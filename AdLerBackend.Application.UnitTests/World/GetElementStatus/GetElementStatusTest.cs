using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.World.GetElementStatus;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.World.GetElementStatus;

public class GetElementStatusTest
{
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [Test]
    public async Task GetLearningElementStatusHandler_Valid_GivesAllScores()
    {
        // Arrange
        var systemUnderTest = new GetLearningElementStatusHandler(_mediator);

        _mediator.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsResponse
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


        // Act
        var result = await systemUnderTest.Handle(new GetElementStatusCommand
        {
            WorldId = 1
        }, CancellationToken.None);

        // Assert
        Assert.AreEqual(1, result.Elements.Count);
    }
}