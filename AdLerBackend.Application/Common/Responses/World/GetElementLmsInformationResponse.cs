#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Responses.World;

public class GetElementLmsInformationResponse
{
    public Modules ElementData { get; set; }
}