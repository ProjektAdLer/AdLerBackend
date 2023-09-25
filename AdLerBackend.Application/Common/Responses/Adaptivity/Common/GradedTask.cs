using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

namespace AdLerBackend.Application.Common.Responses.Adaptivity.Common;

public class GradedTask
{
    public int TaskId { get; set; }
    public string TaskStatus { get; set; } = AdaptivityStates.NotAttempted.ToString();
}