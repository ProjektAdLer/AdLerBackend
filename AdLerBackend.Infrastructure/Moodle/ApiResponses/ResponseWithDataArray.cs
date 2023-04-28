namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

internal class ResponseWithDataArray<T>
{
    public IList<T> Data { get; set; }
}