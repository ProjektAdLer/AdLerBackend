#pragma warning disable CS8618
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Responses.World;

public class GetAllElementsFromLmsWithAdLerIdResponse
{
    public int LmsCourseId { get; set; }
    public IList<ModuleWithId> ModulesWithAdLerId { get; set; }
}

public class ModuleWithId
{
    public int AdLerId { get; set; }
    public Modules LmsModule { get; set; }
}