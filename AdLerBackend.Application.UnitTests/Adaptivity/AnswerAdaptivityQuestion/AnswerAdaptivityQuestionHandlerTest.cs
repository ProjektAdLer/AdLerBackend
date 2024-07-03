using AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Domain.Entities;
using AdLerBackend.Domain.UnitTests.TestingUtils;
using FluentAssertions;
using MediatR;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests.Adaptivity.AnswerAdaptivityQuestion;

public class AnswerAdaptivityQuestionHandlerTest
{
    private  ILMS _lmsMock;
    private  IMediator _mediatorMock;
    private  ISerialization _serializationMock;
    private  IWorldRepository _worldRepositoryMock;
    
    [SetUp]
    // ANF-ID: [BPG14]
    public void Setup()
    {
        _lmsMock = Substitute.For<ILMS>();
        _mediatorMock = Substitute.For<IMediator>();
        _serializationMock = Substitute.For<ISerialization>();
        _worldRepositoryMock = Substitute.For<IWorldRepository>();
    }
    
    [Test]
    // ANF-ID: [BPG14]
    public async Task Handle_Valid_ReturnsResponse()
    {
        // Arrange
        var systemUnderTest = new AnswerAdaptivityQuestionHandler(_lmsMock, _mediatorMock,  _worldRepositoryMock, _serializationMock);
        var QuestionUUID = Guid.NewGuid();
        _mediatorMock.Send(Arg.Any<GetLearningElementCommand>()).Returns(
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
                        new AdaptivityElement()
                        {
                            ElementId = 1,
                            AdaptivityContent = new AdaptivityContent()
                            {
                                AdaptivityTasks = new List<AdaptivityTask>()
                                {
                                    new AdaptivityTask()
                                    {
                                        TaskId = 1,
                                        AdaptivityQuestions = new List<AdaptivityQuestion>()
                                        {
                                            new AdaptivityQuestion()
                                            {
                                                QuestionId = 1,
                                                QuestionUuid = QuestionUUID
                                            }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        });             
        
        _lmsMock.AnswerAdaptivityQuestionsAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<List<AdaptivityAnsweredQuestionTo>>()).Returns(
            new AdaptivityModuleStateResponseAfterAnswer()
            {
                State = AdaptivityStates.Correct,
                Questions = new List<LMSAdaptivityQuestionStateResponse>()
                {
                    new()
                    {
                        Status = AdaptivityStates.Correct
                    }
                },
                Tasks = new List<LMSAdaptivityTaskStateResponse>()
                {
                    new()
                    {
                        State = AdaptivityStates.Correct
                    }
                }
            }
        );
        
        _lmsMock.AnswerAdaptivityQuestionsAsync(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<List<AdaptivityAnsweredQuestionTo>>()).Returns(
            new AdaptivityModuleStateResponseAfterAnswer()
            {
                State = AdaptivityStates.Correct,
                Questions = new List<LMSAdaptivityQuestionStateResponse>()
                {
                    new()
                    {
                        Status = AdaptivityStates.Correct,
                        Answers = new List<LMSAdaptivityQuestionStateResponse.LMSAdaptivityAnswers>()
                        {
                            new LMSAdaptivityQuestionStateResponse.LMSAdaptivityAnswers()
                            {
                                Checked = true,
                                User_Answer_correct = true
                            }
                        }
                    }
                },
                Tasks = new List<LMSAdaptivityTaskStateResponse>()
                {
                    new()
                    {
                        State = AdaptivityStates.Correct
                    },
                },
                
            }
        );
        
        // Act
        var result = await systemUnderTest.Handle(new AnswerAdaptivityQuestionCommand()
        {
            ElementId = 1,
            Answers = new List<bool>(),
            WorldId = 1,
            QuestionId = 1
        }, new System.Threading.CancellationToken());
        
        
        
        // Assert
        result.Should().NotBeNull();
    }
}