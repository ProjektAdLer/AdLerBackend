using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Common.Utils;
using MediatR;

namespace AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;

public class
    AnswerAdaptivityQuestionHandler(
        ILMS lms,
        IMediator mediator,
        IWorldRepository worldRepository,
        ISerialization serialization)
    : IRequestHandler<AnswerAdaptivityQuestionCommand, AnswerAdaptivityQuestionResponse>
{
    public async Task<AnswerAdaptivityQuestionResponse> Handle(AnswerAdaptivityQuestionCommand request,
        CancellationToken cancellationToken)
    {
        // Get LearningElement Activity Id
        var learningElementModule = await mediator.Send(new GetLearningElementCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.WorldId,
            ElementId = request.ElementId
        }, cancellationToken);

        var learningWorld = await worldRepository.GetAsync(request.WorldId);

        var atfObject = serialization.GetObjectFromJsonString<WorldAtfResponse>(learningWorld.AtfJson);

        var adaptivityElementInAtf = atfObject.World.Elements.FirstOrDefault(x =>
            x.ElementId == request.ElementId) as AdaptivityElement;

        var questionUUID = IdExtractor.GetUuidFromQuestionId(request.QuestionId, adaptivityElementInAtf);


        var resultFromLMS = await lms.AnswerAdaptivityQuestionsViaPluginAsync(request.WebServiceToken,
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
                TaskId = IdExtractor.GetTaskIdFromUuid(resultFromLMS.Tasks.First().Uuid,
                    adaptivityElementInAtf!),
                TaskStatus = resultFromLMS.Tasks.First().State.ToString()
            }
        };
    }
}