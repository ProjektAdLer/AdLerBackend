using AdLerBackend.Application.Common.ElementStrategies.GetElementScoreStrategies.GetH5PElementScoreStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetElementLmsInformation;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Element.GetElementScore;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class GetLearningElementScore
{
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [TestCase("h5pactivity", true)]
    [TestCase("h5pactivity", false)]
    [TestCase("url", false)]
    [TestCase("resource", false)]
    public async Task GetLearningElementScore_Valid_GetsScoreFromApi(string modname, bool sucess)
    {
        // Arrange
        var systemUnderTest = new GetElementScoreHandler(_mediator);

        _mediator.Send(Arg.Any<GetElementLmsInformationCommand>())
            .Returns(new GetElementLmsInformationResponse
            {
                ElementData = new Modules
                {
                    contextid = 1,
                    Id = 1,
                    Instance = 1,
                    Name = "name",
                    ModName = modname
                }
            });

        _mediator.Send(Arg.Any<GetH5PElementScoreStrategyCommand>()).Returns(new ElementScoreResponse
        {
            Success = sucess,
            ElementId = 1
        });


        // Act
        var result = await systemUnderTest.Handle(new GetElementScoreCommand
        {
            ElementId = 1,
            lerningWorldId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.Success.Should().Be(sucess);
    }

    [TestCase("INVALID", false)]
    public async Task GetLearningElementScore_InvalidLearningElementType_GetsScoreFromApi(string modname, bool sucess)
    {
        // Arrange
        var systemUnderTest = new GetElementScoreHandler(_mediator);

        _mediator.Send(Arg.Any<GetElementLmsInformationCommand>())
            .Returns(new GetElementLmsInformationResponse
            {
                ElementData = new Modules
                {
                    contextid = 1,
                    Id = 1,
                    Instance = 1,
                    Name = "name",
                    ModName = modname
                }
            });

        _mediator.Send(Arg.Any<GetH5PElementScoreStrategyCommand>()).Returns(new ElementScoreResponse
        {
            Success = sucess,
            ElementId = 1
        });


        // Act
        // Assert
        Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.Handle(
            new GetElementScoreCommand
            {
                ElementId = 1,
                lerningWorldId = 1,
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}