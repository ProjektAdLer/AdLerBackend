using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Element.ScoreElement;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreLearningElementUseCaseTest
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
        var systemUnderTest = new ScoreElementUseCase(_mediator);
        // _mediator.Send(Arg.Any<GetElementLmsInformationCommand>()).Returns(
        //     new GetElementLmsInformationResponse
        //     {
        //         ElementData = new Modules
        //         {
        //             ModName = activityName
        //         }
        //     });

        _mediator.Send(Arg.Any<ScoreH5PElementStrategyCommand>()).Returns(new ScoreElementResponse
        {
            IsSuccess = expected
        });
        _mediator.Send(Arg.Any<ScoreGenericElementStrategyCommand>()).Returns(new ScoreElementResponse
        {
            IsSuccess = expected
        });

        // Act

        var result = await systemUnderTest.Handle(new ScoreElementCommand
        {
            WorldId = 1,
            ElementId = 1,
            ScoreElementParams = new ScoreElementParams(),
            WebServiceToken = "token"
        }, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().Be(expected);
    }

    [TestCase("INVALID", false)]
    public async Task Handle_InvalidElementType_Throws(string activityName, bool expected)
    {
        var systemUnderTest = new ScoreElementUseCase(_mediator);
        // _mediator.Send(Arg.Any<GetElementLmsInformationCommand>()).Returns(
        //     new GetElementLmsInformationResponse
        //     {
        //         ElementData = new Modules
        //         {
        //             ModName = activityName
        //         }
        //     });

        _mediator.Send(Arg.Any<ScoreH5PElementStrategyCommand>()).Returns(new ScoreElementResponse
        {
            IsSuccess = expected
        });
        _mediator.Send(Arg.Any<ScoreGenericElementStrategyCommand>()).Returns(new ScoreElementResponse
        {
            IsSuccess = expected
        });

        // Act
        // Assert
        Assert.ThrowsAsync<NotImplementedException>(async () => await systemUnderTest.Handle(
            new ScoreElementCommand
            {
                WorldId = 1,
                ElementId = 1,
                ScoreElementParams = new ScoreElementParams(),
                WebServiceToken = "token"
            }, CancellationToken.None));
    }
}