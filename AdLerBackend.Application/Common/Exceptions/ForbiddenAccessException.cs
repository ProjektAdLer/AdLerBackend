namespace AdLerBackend.Application.Common.Exceptions;

public class ForbiddenAccessException(string? message) : Exception(message);