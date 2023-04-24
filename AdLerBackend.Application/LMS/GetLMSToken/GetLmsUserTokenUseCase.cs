using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.LMS.GetLMSToken;

public class GetLmsUserTokenUseCase : IRequestHandler<GetLMSTokenCommand, LMSUserTokenResponse>
{
    private readonly ILMS _ilms;

    public GetLmsUserTokenUseCase(ILMS ilms)
    {
        _ilms = ilms;
    }

    public async Task<LMSUserTokenResponse> Handle(GetLMSTokenCommand request, CancellationToken cancellationToken)
    {
        var lmsTokenDto = await _ilms.GetLMSUserTokenAsync(request.UserName, request.Password);
        return lmsTokenDto;
    }
}