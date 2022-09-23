namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class CourseContent
{
    public int Id { get; set; }
    public string Name { get; set; }
    public IList<Modules> Modules { get; set; }
}

public class Modules
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int contextid { get; set; }
    public string ModName { get; set; }
}