namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

public class CourseDeletionWarningsResponse
{
    public string Item { get; set; }
    public int Itemid { get; set; }
    public string Warningcode { get; set; }
    public string Message { get; set; }
}