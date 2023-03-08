namespace AdLerBackend.Application.Common.Exceptions;

public class WorldCreationException : Exception
{
    public WorldCreationException(string? message) : base(message)
    {
    }
}