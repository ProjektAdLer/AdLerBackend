#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.Course;

public class GetWorldOverviewResponse
{
    public IList<WorldResponse> Worlds { get; set; }
}