using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.LMS.GetUserData;

public class GetLmsUserDataUseCase(ILMS ilmsContext) : IRequestHandler<GetLMSUserDataCommand, LMSUserDataResponse>
{
    public async Task<LMSUserDataResponse> Handle(GetLMSUserDataCommand request, CancellationToken cancellationToken)
    {
        var lmsUserDataDto = await ilmsContext.GetLMSUserDataAsync(request.WebServiceToken);
        return lmsUserDataDto;
    }
}