namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class LmsCourseStatusResponse
{
    public IList<LmsElementStatus> ElementScores { get; set; } = new List<LmsElementStatus>();
}