using AdLerBackend.Application.Common;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.LMS.GetUserData;

public record GetLMSUserDataCommand : CommandWithToken<LMSUserDataResponse>;