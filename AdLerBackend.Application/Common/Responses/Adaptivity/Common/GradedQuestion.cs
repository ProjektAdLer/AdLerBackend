using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;

namespace AdLerBackend.Application.Common.Responses.Adaptivity.Common;

// ReSharper disable once ClassNeverInstantiated.Global
public class GradedQuestion
{
    public int Id { get; set; }
    public string Status { get; set; } = AdaptivityStates.NotAttempted.ToString();

    /// <summary>
    ///     If the User did not give any answers yet, this will be null
    /// </summary>
    public IEnumerable<GradedAnswer>? Answers { get; set; }

    /// <summary>
    ///     Represents the answer of the user for this question
    /// </summary>
    public class GradedAnswer
    {
        public bool Checked { get; set; } = false;
        public bool Correct { get; set; } = false;
    }
}