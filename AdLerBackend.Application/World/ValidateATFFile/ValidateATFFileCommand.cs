using MediatR;

namespace AdLerBackend.Application.World.ValidateATFFile;

public record ValidateATFFileCommand : IRequest
{
    public Stream ATFFileStream { get; set; }
}