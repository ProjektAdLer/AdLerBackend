#pragma warning disable CS8618
using MediatR;

namespace AdLerBackend.Application.Common;

public record CommandWithToken<TResponse> : IRequest<TResponse>
{
    public string WebServiceToken { get; set; }
}