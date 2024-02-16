using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.InternalUseCases.GetAllElementsFromLms;
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
    GetAdaptivityElementDetailsHandler : IRequestHandler<GetAdaptivityElementDetailsCommand,
        GetAdaptivityElementDetailsResponse>
{
    private readonly ILMS _lms;
    private readonly IMediator _mediator;
    private readonly ISerialization _serialization;
    private readonly IWorldRepository _worldRepository;

    public GetAdaptivityElementDetailsHandler(ILMS lms, IMediator mediator, IWorldRepository worldRepository,
        ISerialization serialization)
    {
        _lms = lms;
        _mediator = mediator;
        _worldRepository = worldRepository;
        _serialization = serialization;
    }

    public async Task<GetAdaptivityElementDetailsResponse> Handle(GetAdaptivityElementDetailsCommand request,
        CancellationToken cancellationToken)
    {
        var learningElementModule = await _mediator.Send(new GetLearningElementCommand
        {
            WebServiceToken = request.WebServiceToken,
            WorldId = request.LearningWorldId,
            ElementId = request.ElementId
        }, cancellationToken);

        var learningWorld = await _worldRepository.GetAsync(request.LearningWorldId);

        var atfObject = _serialization.GetObjectFromJsonString<WorldAtfResponse>(learningWorld.AtfJson);

        var adaptivityElementInAtf = atfObject.World.Elements.FirstOrDefault(x =>
            x.ElementId == request.ElementId) as AdaptivityElement;

        var adaptivityQuestionDetails =
            await _lms.GetAdaptivityElementDetailsAsync(request.WebServiceToken,
                learningElementModule.LmsModule.Id);

        var adaptivityTaskDetails = await _lms.GetAdaptivityTaskDetailsAsync(request.WebServiceToken,
            learningElementModule.LmsModule.Id);

        var adaptivityElementScore = await _lms.GetElementScoreFromPlugin(request.WebServiceToken,
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