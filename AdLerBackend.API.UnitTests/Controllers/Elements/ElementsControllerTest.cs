using System.Text.Json;
using AdLerBackend.API.Controllers.Elements;
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
    // ANF-ID: [BPG16]
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
            SerializedXapiEvent = JsonSerializer.Serialize(new { test = "test" })
        };

        // Act
        await _controller.ScoreElement(token, elementId, learningWorldId, paramsObj);

        // Assert
        await _mediatorMock.Received(1).Send(Arg.Is<ScoreElementCommand>(x =>
            x.WebServiceToken == token && x.WorldId == learningWorldId && x.ElementId == elementId &&
            x.ScoreElementParams == paramsObj));
    }

    [Test]
    // ANF-ID: [BPG16]
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
}