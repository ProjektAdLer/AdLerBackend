#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class LMSUserDataResponse
{
    public string LMSUserName { get; set; }
    public int UserId { get; set; }
    public string UserEmail { get; set; }
}