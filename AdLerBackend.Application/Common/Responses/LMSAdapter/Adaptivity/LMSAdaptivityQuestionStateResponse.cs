namespace AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

public record LMSAdaptivityQuestionStateResponse
{
    public Guid Uuid { get; init; }

    public AdaptivityStates Status { get; init; } = AdaptivityStates.NotAttempted;

    public IList<LMSAdaptivityAnswers>? Answers { get; init; } = new List<LMSAdaptivityAnswers>();

    public class LMSAdaptivityAnswers
    {
        public bool Checked { get; set; }
        public bool Answer_correct { get; set; }
    }
}