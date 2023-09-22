#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class LMSWorldContentResponse
{
    public IList<Modules> Modules { get; set; }
}

public class Modules
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int contextid { get; set; }
    public string ModName { get; set; }
    public IList<FileContents>? Contents { get; set; }
}

public class FileContents
{
    public string fileUrl { get; set; }
}