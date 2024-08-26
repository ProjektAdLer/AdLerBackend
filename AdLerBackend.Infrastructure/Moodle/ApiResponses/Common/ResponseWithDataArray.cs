namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.Common;

internal class ResponseWithDataArray<T>
{
    public IList<T> Data { get; set; }
}