using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.LMS.GetLMSToken;

public class GetLmsUserTokenUseCase(ILMS ilms) : IRequestHandler<GetLMSTokenCommand, LMSUserTokenResponse>
{
    public async Task<LMSUserTokenResponse> Handle(GetLMSTokenCommand request, CancellationToken cancellationToken)
    {
        var lmsTokenDto = await ilms.GetLMSUserTokenAsync(request.UserName, request.Password);
        return lmsTokenDto;
    }
}