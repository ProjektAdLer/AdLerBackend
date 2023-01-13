#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Responses.Course;

public class GetLearningElementLmsInformationResponse
{
    public Modules LearningElementData { get; set; }
}