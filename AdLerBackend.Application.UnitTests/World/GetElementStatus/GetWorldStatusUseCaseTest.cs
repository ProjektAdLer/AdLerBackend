using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.World.GetWorldStatus;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.World.GetElementStatus;

public class GetWorldStatusUseCaseTest
{
    private ILMS _ilms;
    private IMediator _mediator;

    // ANF-ID: [BPG11]
    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
        _ilms = Substitute.For<ILMS>();
    }

    [Test]
    // ANF-ID: [BPG11]
    public async Task GetWorldStatusHandler_Valid_GivesAllScores()
    {
        // Arrange
        var systemUnderTest = new GetWorldStatusUseCase(_mediator, _ilms);

        _mediator.Send(Arg.Any<GetAllElementsFromLmsCommand>()).Returns(
            new GetAllElementsFromLmsWithAdLerIdResponse
            {
                LmsCourseId = 1337,
                ElementAggregations = new List<AdLerLmsElementAggregation>
                {
                    new()
                    {
                        AdLerElement = new Application.Common.Responses.World.Element
                        {
                            ElementId = 1
                        },
                        LmsModule = new LmsModule
                        {
                            contextid = 1,
                            Id = 1,
                            Name = "Name",
                            ModName = "h5pactivity"
                        }
                    },
                    new()
                    {
                        AdLerElement = new BaseElement
                        {
                            ElementId = 2
                        },
                        IsLocked = true
                    }
                }
            });

        _ilms.GetCourseStatusViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(
            new LmsCourseStatusResponse
            {
                ElementScores = new List<LmsElementStatus>
                {
                    new()
                    {
                        HasScored = true,
                        ModuleId = 1
                    },
                    new()
                    {
                        HasScored = false,
                        ModuleId = 20
                    }
                }
            });


        // Act
        var result = await systemUnderTest.Handle(new GetWorldStatusCommand
        {
            WorldId = 1337
        }, CancellationToken.None);

        // Assert
        Assert.That(result.Elements, Has.Count.EqualTo(2));
        Assert.That(result.Elements[0].ElementId, Is.EqualTo(1));
        Assert.That(result.WorldId, Is.EqualTo(1337));
    }
}