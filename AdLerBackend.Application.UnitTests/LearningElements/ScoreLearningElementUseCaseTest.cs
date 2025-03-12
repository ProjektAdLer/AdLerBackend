using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreGenericLearningElementStrategy;
using AdLerBackend.Application.Common.ElementStrategies.ScoreElementStrategies.ScoreH5PStrategy;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Element.ScoreElement;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.LearningElements;

public class ScoreLearningElementUseCaseTest
{
    private IMediator _mediator;

    // ANF-ID: [BPG13, BPG12]
    [SetUp]
    public void Setup()
    {
        _mediator = Substitute.For<IMediator>();
    }

    [TestCase("h5p", true)]
    [TestCase("video", true)]
    [TestCase("text", false)]
    [TestCase("image", false)]
    [TestCase("text", false)]
    [TestCase("pdf", false)]
    public async Task Handle_CallsStrategy(string activityName, bool expected)
    {
        var systemUnderTest = new ScoreElementUseCase(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = 1,
                    ElementCategory = activityName
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name"
                }
            }
        );

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

    // ANF-ID: [BPG13, BPG12]
    [TestCase("INVALID", false)]
    public async Task Handle_InvalidElementType_Throws(string activityName, bool expected)
    {
        var systemUnderTest = new ScoreElementUseCase(_mediator);

        _mediator.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = 1
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = activityName
                }
            }
        );

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