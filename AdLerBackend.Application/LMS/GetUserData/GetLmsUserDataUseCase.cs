using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using MediatR;

namespace AdLerBackend.Application.LMS.GetUserData;

public class GetLmsUserDataUseCase : IRequestHandler<GetLMSUserDataCommand, LMSUserDataResponse>
{
    private readonly ILMS _ilmsContext;

    public GetLmsUserDataUseCase(ILMS ilmsContext)
    {
        _ilmsContext = ilmsContext;
    }

    public async Task<LMSUserDataResponse> Handle(GetLMSUserDataCommand request, CancellationToken cancellationToken)
    {
        var lmsUserDataDto = await _ilmsContext.GetLMSUserDataAsync(request.WebServiceToken);
        return lmsUserDataDto;
    }
}