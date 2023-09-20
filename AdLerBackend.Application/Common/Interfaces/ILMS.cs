using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Responses.Adaptivity;
using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILMS
{
    /// <summary>
    ///     Gets the LMS User Data for a given Webserice Token
    /// </summary>
    /// <param name="token">LMS Webservice Token</param>
    /// <returns>LMS User Data6</returns>
    Task<LMSUserDataResponse> GetLMSUserDataAsync(string token);

    /// <summary>
    ///     Gets the LMS Webservice Token for a given Account
    /// </summary>
    /// <param name="userName">LMS User Name</param>
    /// <param name="password">LMS User Password</param>
    /// <returns>The LMS Token</returns>
    Task<LMSUserTokenResponse> GetLMSUserTokenAsync(string userName, string password);

    /// <summary>
    ///     Gets the Contents of a World
    /// </summary>
    /// <param name="token">Token of the LMS User</param>
    /// <param name="worldId">ID of the World</param>
    /// <returns>All User-Visible Contents of a World as Array</returns>
    Task<WorldContent[]> GetWorldContentAsync(string token, int worldId);

    /// <summary>
    ///     Gets all Worlds that the User is enrolled in
    /// </summary>
    /// <param name="token">The Users Webservice Token</param>
    /// <returns></returns>
    Task<LMSWorldListResponse> GetWorldsForUserAsync(string token);

    /**
     * Uploads a Course Backup to the LMS
     * The Course is being send as Base64 encoded String
     * @param token The LMS Webservice Token
     * @param backupFileStream The Stream of the Backup File
     */
    Task<LMSCourseCreationResponse> UploadCourseWorldToLMS(string token, Stream backupFileStream);

    /// <summary>
    ///     Gets the Score of an Element from the LMS via the Plugin
    /// </summary>
    /// <param name="token">Webservice Token</param>
    /// <param name="elementId">The Module ID of the Element</param>
    /// <returns></returns>
    Task<bool> GetElementScoreFromPlugin(string token, int elementId);

    /// <summary>
    ///     Scores an Element via the Plugin
    /// </summary>
    /// <param name="token">Webservice Token</param>
    /// <param name="elementId">The Module ID if the Element</param>
    /// <returns></returns>
    Task<bool> ScoreGenericElementViaPlugin(string token, int elementId);

    /// <summary>
    ///     Processes an XAPI Statement via the Plugin
    /// </summary>
    /// <param name="token">Webservice Token</param>
    /// <param name="statement">The Statement of the XAPI Request</param>
    /// <returns></returns>
    Task<bool> ProcessXApiViaPlugin(string token, string statement);

    /// <summary>
    ///     Gets the Course Status via the Plugin
    /// </summary>
    /// <param name="token"></param>
    /// <param name="courseId"></param>
    /// <returns></returns>
    Task<LmsCourseStatusResponse> GetCourseStatusViaPlugin(string token, int courseId);

    /// <summary>
    ///     Gets the LMS IDs for a given list of UUID
    /// </summary>
    /// <param name="uuids">List of UUIDs to be </param>
    /// <returns></returns>
    Task<IEnumerable<LmsUuidResponse>> GetLmsElementIdsByUuidsAsync(string token, int courseInstanceId,
        IEnumerable<string> uuids);

    /// <summary>
    ///     Deletes a Course from the LMS
    /// </summary>
    /// <returns></returns>
    Task DeleteCourseAsync(string token, int worldId);

    /// <summary>
    ///     Give an Answer for an Adaptivity Question to the LMS
    /// </summary>
    /// <param name="token"></param>
    /// <param name="elementId">The Module ID of the Element</param>
    /// <param name="answeredQuestions"></param>
    /// <returns> A list of the given Answers and there State of correctness</returns>
    Task<IEnumerable<AdaptivityQuestionStateResponse>> AnswerAdaptivityQuestionsAsync(string token, int elementId,
        IEnumerable<AdaptivityAnsweredQuestionTo> answeredQuestions);

    /// <summary>
    ///     Gets all Question in an Adaptivity Element with there State of correctness
    /// </summary>
    /// <param name="token"></param>
    /// <param name="elementId"></param>
    /// <returns></returns>
    Task<IEnumerable<AdaptivityQuestionStateResponse>> GetAdaptivityElementDetailsAsync(string token, int elementId);
}