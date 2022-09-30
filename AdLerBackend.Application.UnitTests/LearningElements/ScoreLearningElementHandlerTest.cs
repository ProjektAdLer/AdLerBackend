using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElementLmsInformation;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.
    ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.LearningElementStrategies.ScoreLearningElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Responses.Course;
using AdLerBackend.Application.Common.Responses.LearningElements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.LearningElement.ScoreLearningElement;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreLearningElementHandlerTest
{
    private IMediator _mediator;

    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [TestCase("h5pactivity", true)]
    [TestCase("url", true)]
    [TestCase("resource", false)]
    public async Task Handle_CallsStrategy(string activityName, bool expected)
    {
        var systemUnderTest = new ScoreLearningElementHandler(_mediator);
        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>()).Returns(
            new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    ModName = activityName
                }
            });

        _mediator.Send(Arg.Any<ScoreH5PElementStrategyCommand>()).Returns(new ScoreLearningElementResponse
        {
            isSuceess = expected
        });
        _mediator.Send(Arg.Any<ScoreGenericLearningElementStrategyCommand>()).Returns(new ScoreLearningElementResponse
        {
            isSuceess = expected
        });

        // Act

        var result = await systemUnderTest.Handle(new ScoreLearningElementCommand
        {
            CourseId = 1,
            ElementId = 1,
            ScoreElementParams = new ScoreElementParams(),
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.isSuceess.Should().Be(expected);
    }

    [TestCase("INVALID", false)]
    public async Task Handle_InvalidElementType_Throws(string activityName, bool expected)
    {
        var systemUnderTest = new ScoreLearningElementHandler(_mediator);
        _mediator.Send(Arg.Any<GetLearningElementLmsInformationCommand>()).Returns(
            new GetLearningElementLmsInformationResponse
            {
                LearningElementData = new Modules
                {
                    ModName = activityName
                }
            });

        _mediator.Send(Arg.Any<ScoreH5PElementStrategyCommand>()).Returns(new ScoreLearningElementResponse
        {
            isSuceess = expected
        });
        _mediator.Send(Arg.Any<ScoreGenericLearningElementStrategyCommand>()).Returns(new ScoreLearningElementResponse
        {
            isSuceess = expected
        });

        // Act
        // Assert
        Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.Handle(
            new ScoreLearningElementCommand
            {
                CourseId = 1,
                ElementId = 1,
                ScoreElementParams = new ScoreElementParams(),
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}