using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using MediatR;

namespace AdLerBackend.Application.Adaptivity.GetAdaptivityModuleQuestionDetails;

public class
    GetAdaptivityElementDetailsHandler : IRequestHandler<GetAdaptivityElementDetailsCommand,
        GetAdaptivityElementDetailsResponse>
{
    public async Task<GetAdaptivityElementDetailsResponse> Handle(GetAdaptivityElementDetailsCommand request,
        CancellationToken cancellationToken)
    {
        return new GetAdaptivityElementDetailsResponse
        {
            Element = new ElementScoreResponse
            {
                ElementId = 1,
                Success = true
            },
            Questions = new List<GradedQuestion>
            {
                new()
                {
                    Answers = new List<GradedQuestion.GradedAnswer>
                    {
                        new()
                        {
                            Correct = true,
                            Checked = false
                        }
                    }
                }
            },
            Tasks = new List<GradedTask>
            {
                new()
                {
                    TaskId = 1,
                    TaskStatus = AdaptivityStates.NotAttempted.ToString()
                }
            }
        };
    }
}