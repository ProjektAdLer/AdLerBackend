#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.DTOs;

public class H5PDto
{
    public Stream? H5PFile { get; init; }
    public string? H5PFileName { get; init; }
}