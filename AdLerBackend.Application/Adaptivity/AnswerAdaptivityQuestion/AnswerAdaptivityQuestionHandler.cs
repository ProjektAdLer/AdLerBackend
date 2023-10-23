using System.Text.Json;
using AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Common.Responses.World;
using MediatR;

namespace AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;

public class
    AnswerAdaptivityQuestionHandler : IRequestHandler<AnswerAdaptivityQuestionCommand, AnswerAdaptivityQuestionResponse>
{
    private readonly ILMS _lms;
    private readonly IMediator _mediator;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public AnswerAdaptivityQuestionHandler(ILMS lms, IMediator mediator, IWorldRepository worldRepository,
        ISerialization serialization)
    {
        _lms = lms;
        _mediator = mediator;
        _worldRepository = worldRepository;
        _serialization = serialization;
    }

    public async Task<AnswerAdaptivityQuestionResponse> Handle(AnswerAdaptivityQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var learningElementModules = await _mediator.Send(new GetAllElementsFromLmsCommand
        {
            WorldId = request.WorldId,
            WebServiceToken = request.WebServiceToken
        }, cancellationToken);

        // Get LearningElement Activity Id
        var learningElementModule = learningElementModules.ModulesWithAdLerId
            .FirstOrDefault(x => x.AdLerId == request.ElementId);

        var learningWorld = await _worldRepository.GetAsync(request.WorldId);

        var atfObject = _serialization.GetObjectFromJsonString<WorldAtfResponse>(learningWorld.AtfJson);

        var adaptivityElementInAtf = atfObject.World.Elements.FirstOrDefault(x =>
            x.ElementId == request.ElementId) as AdaptivityElement;

        var questionUUID = GetUuidFromQuestionId(request.QuestionId, adaptivityElementInAtf);


        var resultFromLMS = await _lms.AnswerAdaptivityQuestionsAsync(request.WebServiceToken,
            learningElementModule.LmsModule.Id,
            new List<AdaptivityAnsweredQuestionTo>
            {
                new()
                {
                    Uuid = questionUUID.ToString(),
                    Answer = JsonSerializer.Serialize(request.Answers)
                }
            });

        return new AnswerAdaptivityQuestionResponse
        {
            ElementScore = new ElementScoreResponse
            {
                ElementId = request.ElementId,
                Success = resultFromLMS.State == AdaptivityStates.Correct
            },
            GradedQuestion = new GradedQuestion
            {
                Status = resultFromLMS.Questions.First().Status.ToString(),
                Id = request.QuestionId,
                Answers = resultFromLMS.Questions?.First().Answers?.Select(answer => new GradedQuestion.GradedAnswer
                {
                    Checked = answer.Checked,
                    Correct = answer.User_Answer_correct
                }) ?? null
            },
            GradedTask = new GradedTask
            {
                TaskId = GetAdaptivityElementDetailsHandler.GetTaskIdFromUuid(resultFromLMS.Tasks.First().Uuid,
                    adaptivityElementInAtf!),
                TaskStatus = resultFromLMS.Tasks.First().State.ToString()
            }
        };
    }

    public static Guid GetUuidFromQuestionId(int questionId, AdaptivityElement adaptivityElement)
    {
        foreach (var question in from task in adaptivityElement.AdaptivityContent.AdaptivityTasks
                 from question in task.AdaptivityQuestions
                 where question.QuestionId == questionId
                 select question)
            return question.QuestionUuid;

        throw new Exception("No uuid for the Adaptivity Question found!");
    }
}