namespace AdLerBackend.Application.Common.Responses.World;

public class CreateWorldResponse
{
    public string WorldNameInLms { get; set; }
    public string WorldLmsUrl { get; set; }
    public string World3DUrl { get; set; } = "Coming soon :)";
}