using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetLearningElement;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.World;
using AdLerBackend.Application.Common.Utils;
using JetBrains.Annotations;
using MediatR;

namespace AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;

[UsedImplicitly]
public class
    GetAdaptivityElementDetailsHandler(
        ILMS lms,
        IMediator mediator,
        IWorldRepository worldRepository,
        ISerialization serialization)
    : IRequestHandler<GetAdaptivityElementDetailsCommand,
        GetAdaptivityElementDetailsResponse>
{
    public async Task<GetAdaptivityElementDetailsResponse> Handle(GetAdaptivityElementDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var learningElementModule = await mediator.Send(new GetLearningElementCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.LearningWorldId,
            ElementId = request.ElementId
        }, cancellationToken);

        var learningWorld = await worldRepository.GetAsync(request.LearningWorldId);

        var atfObject = serialization.GetObjectFromJsonString<WorldAtfResponse>(learningWorld.AtfJson);

        var adaptivityElementInAtf = atfObject.World.Elements.FirstOrDefault(x =>
            x.ElementId == request.ElementId) as AdaptivityElement;

        var adaptivityQuestionDetails =
            await lms.GetAdaptivityElementDetailsAsync(request.WebServiceToken,
                learningElementModule.LmsModule.Id);

        var adaptivityTaskDetails = await lms.GetAdaptivityTaskDetailsAsync(request.WebServiceToken,
            learningElementModule.LmsModule.Id);

        var adaptivityElementScore = await lms.GetElementScoreFromPlugin(request.WebServiceToken,
            learningElementModule.LmsModule.Id);


        return new GetAdaptivityElementDetailsResponse
        {
            Element = new ElementScoreResponse
            {
                ElementId = request.ElementId,
                Success = adaptivityElementScore
            },
            Questions = adaptivityQuestionDetails.Select(question => new GradedQuestion
            {
                Answers = question.Answers?.Select(answer => new GradedQuestion.GradedAnswer
                {
                    Checked = answer.Checked,
                    Correct = answer.User_Answer_correct
                }) ?? null,
                Id = IdExtractor.GetQuestionIdFromUuid(question.Uuid, adaptivityElementInAtf!),
                Status = question.Status.ToString()
            }),
            Tasks = adaptivityTaskDetails.Select(task => new GradedTask
            {
                TaskId = IdExtractor.GetTaskIdFromUuid(task.Uuid, adaptivityElementInAtf!),
                TaskStatus = task.State.ToString()
            })
        };
    }
}