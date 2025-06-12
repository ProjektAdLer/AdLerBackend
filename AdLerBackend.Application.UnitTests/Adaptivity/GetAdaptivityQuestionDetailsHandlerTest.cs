using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Adaptivity;

// ANF-ID: [BPG15]
public class GetAdaptivityQuestionDetailsHandlerTest
{
    private ILMS _lmsMock;
    private IMediator _mediatorMock;
    private ISerialization _serializationMock;
    private IWorldRepository _worldRepositoryMock;

    [SetUp]
    public void Setup()
    {
        _lmsMock = Substitute.For<ILMS>();
        _mediatorMock = Substitute.For<IMediator>();
        _serializationMock = Substitute.For<ISerialization>();
        _worldRepositoryMock = Substitute.For<IWorldRepository>();
    }

    [Test]
    // ANF-ID: [BPG15]
    public async Task Handle_Valid_ReturnsResponse()
    {
        // Arrange
        var systemUnderTest =
            new GetAdaptivityElementDetailsHandler(_lmsMock, _mediatorMock, _worldRepositoryMock, _serializationMock);
        var elementId = 1;
        var worldId = 1;
        var moduleId = 1;
        var questionUuid = Guid.NewGuid();
        var taskUuid = Guid.NewGuid();

        _mediatorMock.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = elementId
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = moduleId,
                    Name = "name",
                    ModName = "h5pactivity"
                }
            }
        );

        _worldRepositoryMock.GetAsync(Arg.Any<int>()).Returns(
            WorldEntityFactory.CreateWorldEntity()
        );

        _serializationMock.GetObjectFromJsonString<WorldAtfResponse>(Arg.Any<string>()).Returns(
            new WorldAtfResponse
            {
                World = new Application.Common.Responses.World.World
                {
                    Elements = new List<BaseElement>
                    {
                        new AdaptivityElement
                        {
                            ElementId = elementId,
                            AdaptivityContent = new AdaptivityContent
                            {
                                AdaptivityTasks = new List<AdaptivityTask>
                                {
                                    new()
                                    {
                                        TaskId = 1,
                                        TaskUuid = taskUuid,
                                        AdaptivityQuestions = new List<AdaptivityQuestion>
                                        {
                                            new()
                                            {
                                                QuestionId = 1,
                                                QuestionUuid = questionUuid
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        );

        _lmsMock.GetAdaptivityElementDetailsViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(
            new List<LMSAdaptivityQuestionStateResponse>
            {
                new()
                {
                    Status = AdaptivityStates.Correct,
                    Uuid = questionUuid,
                    Answers = new List<LMSAdaptivityQuestionStateResponse.LMSAdaptivityAnswers>
                    {
                        new()
                        {
                            Checked = true,
                            User_Answer_correct = true
                        }
                    }
                }
            }
        );

        _lmsMock.GetAdaptivityTaskDetailsViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(
            new List<LMSAdaptivityTaskStateResponse>
            {
                new()
                {
                    State = AdaptivityStates.Correct,
                    Uuid = taskUuid
                }
            }
        );

        _lmsMock.GetElementScoreViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(true);

        // Act
        var result = await systemUnderTest.Handle(new GetAdaptivityElementDetailsCommand
        {
            WebServiceToken = "token",
            LearningWorldId = worldId,
            ElementId = elementId
        }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Element.Should().NotBeNull();
        result.Element.ElementId.Should().Be(elementId);
        // result.Element.Success.Should().Be(1.0f);

        result.Questions.Should().NotBeNull();
        result.Questions.Should().HaveCount(1);
        result.Questions.First().Id.Should().Be(1);
        result.Questions.First().Status.Should().Be("Correct");
        result.Questions.First().Answers.Should().HaveCount(1);
        result.Questions.First().Answers.First().Checked.Should().BeTrue();
        result.Questions.First().Answers.First().Correct.Should().BeTrue();

        result.Tasks.Should().NotBeNull();
        result.Tasks.Should().HaveCount(1);
        result.Tasks.First().TaskId.Should().Be(1);
        result.Tasks.First().TaskStatus.Should().Be("Correct");
    }

    [Test]
    // ANF-ID: [BPG15]
    public async Task Handle_NoAdaptivityElement_ReturnsEmptyResponse()
    {
        // Arrange
        var systemUnderTest =
            new GetAdaptivityElementDetailsHandler(_lmsMock, _mediatorMock, _worldRepositoryMock, _serializationMock);
        var elementId = 1;
        var worldId = 1;

        _mediatorMock.Send(Arg.Any<GetLearningElementCommand>()).Returns(
            new AdLerLmsElementAggregation
            {
                IsLocked = false,
                AdLerElement = new BaseElement
                {
                    ElementId = elementId
                },
                LmsModule = new LmsModule
                {
                    contextid = 1,
                    Id = 1,
                    Name = "name",
                    ModName = "h5pactivity"
                }
            }
        );

        _worldRepositoryMock.GetAsync(Arg.Any<int>()).Returns(
            WorldEntityFactory.CreateWorldEntity()
        );

        _serializationMock.GetObjectFromJsonString<WorldAtfResponse>(Arg.Any<string>()).Returns(
            new WorldAtfResponse
            {
                World = new Application.Common.Responses.World.World
                {
                    Elements = new List<BaseElement>
                    {
                        new()
                        {
                            ElementId = elementId
                        }
                    }
                }
            }
        );

        _lmsMock.GetAdaptivityElementDetailsViaPluginAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(new List<LMSAdaptivityQuestionStateResponse>());
        _lmsMock.GetAdaptivityTaskDetailsViaPluginAsync(Arg.Any<string>(), Arg.Any<int>())
            .Returns(new List<LMSAdaptivityTaskStateResponse>());
        _lmsMock.GetElementScoreViaPluginAsync(Arg.Any<string>(), Arg.Any<int>()).Returns(false);

        // Act
        var result = await systemUnderTest.Handle(new GetAdaptivityElementDetailsCommand
        {
            WebServiceToken = "token",
            LearningWorldId = worldId,
            ElementId = elementId
        }, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Element.Should().NotBeNull();
        result.Element.ElementId.Should().Be(elementId);
        result.Element.Success.Should().Be(false);
        result.Questions.Should().BeEmpty();
        result.Tasks.Should().BeEmpty();
    }
}