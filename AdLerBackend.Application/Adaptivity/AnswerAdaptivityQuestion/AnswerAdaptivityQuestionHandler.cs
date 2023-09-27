using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.Adaptivity.Common;
using AdLerBackend.Application.Common.Responses.Elements;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using MediatR;

namespace AdLerBackend.Application.Adaptivity.AnswerAdaptivityQuestion;

public class
    AnswerAdaptivityQuestionHandler : IRequestHandler<AnswerAdaptivityQuestionCommand, AnswerAdaptivityQuestionResponse>
{
    public async Task<AnswerAdaptivityQuestionResponse> Handle(AnswerAdaptivityQuestionCommand request,
        CancellationToken cancellationToken)
    {
        return new AnswerAdaptivityQuestionResponse
        {
            ElementScore = new ElementScoreResponse
            {
                ElementId = 123,
                Success = true
            },
            GradedQuestion = new GradedQuestion
            {
                Status = AdaptivityStates.Correct.ToString(),
                Id = 1234,
                Answers = new List<GradedQuestion.GradedAnswer>
                {
                    new()
                    {
                        Correct = true,
                        Checked = true
                    }
                }
            },
            GradedTask = new GradedTask
            {
                TaskId = 1234,
                TaskStatus = AdaptivityStates.Correct.ToString()
            }
        };
    }
}