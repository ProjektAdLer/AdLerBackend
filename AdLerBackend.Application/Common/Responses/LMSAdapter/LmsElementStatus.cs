namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class LmsElementStatus
{
    public int ModuleId { get; set; }
    public bool HasScored { get; set; }
    public int Score { get; set; }
}