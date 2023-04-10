using AdLerBackend.Application.Common.Responses.LMSAdapter;

namespace AdLerBackend.Application.Common.Interfaces;

public interface ILMS
{
    /// <summary>
    ///     Gets the LMS User Data for a given Webserice Token
    /// </summary>
    /// <param name="token">LMS Webservice Token</param>
    /// <returns>LMS User Data6</returns>
    Task<LMSUserDataResponse> GetLmsUserDataAsync(string token);

    /// <summary>
    ///     Gets the LMS Webservice Token for a given Account
    /// </summary>
    /// <param name="userName">LMS User Name</param>
    /// <param name="password">LMS User Password</param>
    /// <returns>The LMS Token</returns>
    Task<LMSUserTokenResponse> GetLmsUserTokenAsync(string userName, string password);

    /// <summary>
    ///     Searches all Worlds, that are avalibale for the given LMS User
    /// </summary>
    /// <param name="token">Token of the LMS User</param>
    /// <param name="searchString">The World to get Searched for</param>
    /// <param name="limitToEnrolled">if the Search should only return courses where the user in enrolled in</param>
    /// <returns>A List of all found Courses</returns>
    Task<LMSWorldListResponse> SearchWorldsAsync(string token, string searchString, bool limitToEnrolled = false);

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

    /// <summary>
    ///     Determines whether the given User is an admin in the LMS System
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<bool> IsLmsAdminAsync(string token);

    /// <summary>
    ///     Processes an XAPI Statement
    /// </summary>
    /// <param name="token"></param>
    /// <param name="statement">The Statement of the XAPI Request</param>
    /// <returns>Returns True, if the Statement hase been processed successfully </returns>
    Task<bool> ProcessXApiStatementAsync(string token, string statement);

    Task<H5PAttempts> GetH5PAttemptsAsync(string token, int h5PActivityId);

    Task<bool> ScoreGenericElement(string token, int elementId);

    /**
     * Uploads a Course Backup to the LMS
     * The Course is being send as Base64 encoded String
     * @param token The LMS Webservice Token
     * @param backupFileStream The Stream of the Backup File
     */
    // ReSharper disable once UnusedMemberInSuper.Global
    Task<int> UploadCourseWorldToLms(string token, Stream backupFileStream);
}