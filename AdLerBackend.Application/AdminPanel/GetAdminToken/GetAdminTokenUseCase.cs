using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.AdminPanel.GetAdminToken;

public class GetAdminTokenUseCase(ILMS lmsService) : IRequestHandler<GetAdminTokenCommand, LMSUserTokenResponse>
{
    public Task<LMSUserTokenResponse> Handle(GetAdminTokenCommand request, CancellationToken cancellationToken)
    {
        return lmsService.GetLMSAdminTokenAsync(request.UserName, request.Password);
    }
}