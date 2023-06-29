using MediatR;

namespace AdLerBackend.Application.World.WorldManagement.DeleteWorld;

public record DeleteWorldCommand : IRequest<bool>
{
    public string WebServiceToken { get; set; }
    public int WorldId { get; init; }
}