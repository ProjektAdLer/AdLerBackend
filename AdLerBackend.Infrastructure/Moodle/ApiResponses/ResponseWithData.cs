namespace AdLerBackend.Infrastructure.Moodle.ApiResponses;

internal class ResponseWithData<T>
{
    public T Data { get; set; }
}