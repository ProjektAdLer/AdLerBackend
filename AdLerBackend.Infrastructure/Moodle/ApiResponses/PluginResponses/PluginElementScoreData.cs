namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.PluginResponses;

internal class PluginElementScoreData
{
    public int Module_id { get; set; }
    public int? Score { get; set; }
    public bool Completed { get; set; }
}