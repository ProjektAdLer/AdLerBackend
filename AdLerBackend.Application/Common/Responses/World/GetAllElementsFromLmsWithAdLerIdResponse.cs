#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Responses.World;

public class GetAllElementsFromLmsWithAdLerIdResponse
{
    public int LmsCourseId { get; set; }
    public int AdLerWorldId { get; set; }
    public IList<AdLerLmsElementAggregation> ElementAggregations { get; set; }
}

public class AdLerLmsElementAggregation
{
    public BaseElement AdLerElement { get; set; }
    public LmsModule LmsModule { get; set; }
    public bool IsLocked { get; set; }
}