using AdLerBackend.API.Controllers.Elements;
using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using MediatR;
using NSubstitute;

namespace AdLerBackend.API.UnitTests.Controllers.Elements;

public class AdaptivityElementControllerTests
{
    private AdaptivityElementController _controller;
    private IMediator _mediatorMock;

    [SetUp]
    // ANF-ID: [BPG16]
    public void SetUp()
    {
        _mediatorMock = Substitute.For<IMediator>();
        _controller = new AdaptivityElementController(_mediatorMock);
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