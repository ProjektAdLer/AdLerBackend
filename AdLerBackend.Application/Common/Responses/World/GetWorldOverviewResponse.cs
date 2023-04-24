#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.World;

public class GetWorldOverviewResponse
{
    public IList<WorldResponse> Worlds { get; set; }
}