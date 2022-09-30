using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Responses.Course;

public class GetAllLearningElementsFromLmsResponse
{
    public IList<ModuleWithId> ModulesWithID { get; set; }
}

public class ModuleWithId
{
    public int Id { get; set; }
    public Modules Module { get; set; }
}