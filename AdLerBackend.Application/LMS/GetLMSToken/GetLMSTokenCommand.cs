using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

#pragma warning disable CS8618
namespace AdLerBackend.Application.LMS.GetLMSToken;

public record GetLMSTokenCommand : IRequest<LMSUserTokenResponse>
{
    public string UserName { get; init; }
    public string Password { get; init; }
}