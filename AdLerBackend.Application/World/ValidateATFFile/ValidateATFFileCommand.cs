using MediatR;

namespace AdLerBackend.Application.World.ValidateATFFile;

public record ValidateATFFileCommand : IRequest<Unit>
{
    public Stream ATFFileStream { get; set; }
}