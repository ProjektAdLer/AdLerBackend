using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.LearningElementStrategies.H5PLearningElementStrategy;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.GetLearningElementScore;
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
    [TestCase("Noh5pactivity", true)]
    [TestCase("Noh5pactivity", false)]
    public async Task GetLearningElementScore_Valid_GetsScoreFromApi(string modname, bool sucess)
    {
        // Arrange
        var systemUnderTest = new GetLearningElementScoreHandler(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>())
            .Returns(new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    contextid = 1,
                    Id = 1,
                    Instance = 1,
                    Name = "name",
                    ModName = modname
                }
            });

        _mediator.Send(Arg.Any<H5PLearningElementStrategyCommand>()).Returns(new LearningElementScoreResponse
        {
            successss = sucess,
            ElementId = 1
        });


        // Act
        var result = await systemUnderTest.Handle(new GetLearningElementScoreCommand
        {
            learningElementId = 1,
            lerningWorldId = 1,
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.successss.Should().Be(sucess);
    }
}