namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

internal class GeneralUserDataResponse
{
    public string Username { get; set; }
    public bool UserIsSiteAdmin { get; set; }
    public int Userid { get; set; }
}