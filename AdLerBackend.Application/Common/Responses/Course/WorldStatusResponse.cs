#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.Elements;

namespace AdLerBackend.Application.Common.Responses.Course;

public class WorldStatusResponse
{
    public int WorldId { get; set; }
    public IList<ElementScoreResponse> Elements { get; set; }
}