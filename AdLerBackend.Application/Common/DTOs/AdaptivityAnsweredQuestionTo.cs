namespace AdLerBackend.Application.Common.DTOs;

/// <summary>
///     The Answer to an Adaptivity Question
/// </summary>
public record AdaptivityAnsweredQuestionTo
{
    /// <summary>
    ///     Since the LMS has the possibility to have an UUID for a single Question, we can use it to identify the Question
    /// </summary>
    public string Uuid { get; init; }

    /// <summary>
    ///     The Answer encoded in a String for Compatibility future versions of the Adaptivity LmsModule
    /// </summary>
    public string Answer { get; init; }
}