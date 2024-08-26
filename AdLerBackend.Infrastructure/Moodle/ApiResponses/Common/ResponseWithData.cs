namespace AdLerBackend.Infrastructure.Moodle.ApiResponses.Common;

internal class ResponseWithData<T>
{
    public T Data { get; set; }
}