using System.Text.Json;
using AdLerBackend.API.Controllers.Elements;
using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Element.GetElementScore;
using AdLerBackend.Application.Element.ScoreElement;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Elements;

public class ElementsControllerTest
{
    private ElementsController _controller;
    private IMediator _mediatorMock;

    [SetUp]
    public void SetUp()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _controller = new ElementsController(_mediatorMock);
    }

    [Test]
    public async Task ScoreElement_ForwardsCall()
    {
        // Arrange
        const string token = "token";
        const int learningWorldId = 1;
        const int elementId = 2;
        var paramsObj = new ScoreElementParams
        {
            //any serialized object
            SerializedXapiEvent = JsonSerializer.Serialize(new {test = "test"})
        };

        // Act
        await _controller.ScoreElement(token, elementId, learningWorldId, paramsObj);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Is<ScoreElementCommand>(x =>
            x.WebServiceToken == token && x.WorldId == learningWorldId && x.ElementId == elementId &&
            x.ScoreElementParams == paramsObj));
    }

    [Test]
    public async Task GetElementScore_ForwardsCall()
    {
        // Arrange
        const string token = "token";
        const int learningWorldId = 1;
        const int elementId = 2;

        // Act
        await _controller.GetElementScore(token, elementId, learningWorldId);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Is<GetElementScoreCommand>(x =>
            x.WebServiceToken == token && x.LearningWorldId == learningWorldId && x.ElementId == elementId));
    }

    [Test]
    public async Task AnswerAdaptivityQuestion_ForwardsCall()
    {
        // Arrange
        const string token = "token";
        const int learningWorldId = 1;
        const int elementId = 2;
        const int questionId = 3;
        var answers = new[] {true, false};

        // Act
        await _controller.AnswerAdaptivityQuestion(token, learningWorldId, elementId, questionId, answers);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Is<AnswerAdaptivityQuestionCommand>(x =>
            x.WebServiceToken == token && x.WorldId == learningWorldId && x.ElementId == elementId &&
            x.QuestionId == questionId && x.Answers == answers));
    }

    [Test]
    public async Task GetAdaptivityQuestions_ForwardsCall()
    {
        // Arrange
        const string token = "token";
        const int learningWorldId = 1;
        const int elementId = 2;

        // Act
        await _controller.GetAdaptivityQuestions(token, learningWorldId, elementId);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Is<GetAdaptivityElementDetailsCommand>(x =>
            x.WebServiceToken == token && x.LearningWorldId == learningWorldId && x.ElementId == elementId));
    }
}