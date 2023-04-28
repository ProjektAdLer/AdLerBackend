namespace AdLerBackend.Infrastructure.Moodle;

internal class PostToMoodleOptions
{
    public Endpoints Endpoint { get; set; } = Endpoints.Webservice;

    internal enum Endpoints
    {
        Webservice,
        Login
    }
}