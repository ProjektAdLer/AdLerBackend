using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Infrastructure.Moodle.ApiResponses;
using Microsoft.Extensions.Configuration;

namespace AdLerBackend.Infrastructure.Moodle;

public class MoodleWebApi : ILMS
{
    private readonly HttpClient _client;
    private readonly IConfiguration _configuration;
    private readonly MoodleUtils _moodleUtils;


    public MoodleWebApi(HttpClient client, IConfiguration configuration, MoodleUtils moodleUtils)
    {
        _client = client;
        _configuration = configuration;
        _moodleUtils = moodleUtils;
    }

    public async Task<LMSUserTokenResponse> GetLMSUserTokenAsync(string userName, string password)
    {
        var resp = await MoodleCallAsync<UserTokenResponse>(new Dictionary<string, HttpContent>
        {
            {"username", new StringContent(userName)},
            {"password", new StringContent(password)},
            {"service", new StringContent("moodle_mobile_app")}
        }, new PostToMoodleOptions
        {
            Endpoint = PostToMoodleOptions.Endpoints.Login
        });

        return new LMSUserTokenResponse
        {
            LMSToken = resp.Token
        };
    }

    public async Task<WorldContent[]> GetWorldContentAsync(string token, int worldId)
    {
        var resp = await MoodleCallAsync<WorldContent[]>(new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("core_course_get_contents")},
            {"courseid", new StringContent(worldId.ToString())}
        });
        return resp;
    }

    public async Task<LMSWorldListResponse> GetWorldsForUserAsync(string token)
    {
        return await MoodleCallAsync<LMSWorldListResponse>(new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("core_course_search_courses")},
            {"criterianame", new StringContent("search")},
            {"criteriavalue", new StringContent("")},
            {"limittoenrolled", new StringContent("1")}
        });
    }

    public async Task<bool> IsLMSAdminAsync(string token)
    {
        var userData = await GetLMSUserDataAsync(token);
        return userData.IsAdmin;
    }

    public async Task<LMSCourseCreationResponse> UploadCourseWorldToLMS(string token, Stream backupFileStream)
    {
        var fileContent = new StreamContent(backupFileStream);

        var multiPartContent = new MultipartFormDataContent
        {
            {new StringContent(token), "wstoken"},
            {new StringContent("local_adler_upload_course"), "wsfunction"},
            {fileContent, "mbz", "filename.mbz"} // Set filename.mbz as the name of the file
        };

        var response = await MoodleCallAsync<ResponseWithData<PluginCourseCreationResponse>>(multiPartContent);

        return new LMSCourseCreationResponse
        {
            CourseLmsId = response.Data.Course_Id,
            CourseLmsName = response.Data.Course_Fullname
        };
    }

    public async Task<bool> GetElementScoreFromPlugin(string token, int elementId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_get_element_scores")},
                {"module_ids[0]", new StringContent(elementId.ToString())}
            });

        // Todo replace with the actual score
        return response.Data[0].Score > 0;
    }

    public async Task<bool> ScoreGenericElementViaPlugin(string token, int elementId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_primitive_learning_element")},
                {"module_id", new StringContent(elementId.ToString())},
                {"is_completed", new StringContent("1")}
            });

        return response.Data[0].Score > 0;
    }

    public async Task<bool> ProcessXApiViaPlugin(string token, string statement)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_h5p_learning_element")},
                {"xapi", new StringContent("[" + statement + "]")}
            });

        return response.Data[0].Score > 0;
    }

    public async Task<LmsCourseStatusResponse> GetCourseStatusViaPlugin(string token, int courseId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_get_course_scores")},
                {"course_id", new StringContent(courseId.ToString())}
            });

        var courseStatus = new LmsCourseStatusResponse
        {
            ElementScores = response.Data
                .Where(x => x.Score != null) // Filter out non-AdLer Courses
                .Select(x => new LmsElementStatus // Map to LMS Element Status
                {
                    ModuleId = x.Module_id,
                    HasScored = x.Score > 0
                })
                .ToList()
        };

        return courseStatus;
    }

    public Task<IList<LmsUuidResponse>> GetLmsIdsByUuidsAsync(string token, IList<string> uuids)
    {
        throw new NotImplementedException();
    }

    public async Task<LMSUserDataResponse> GetLMSUserDataAsync(string token)
    {
        var generalInformationResponse = await MoodleCallAsync<GeneralUserDataResponse>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("core_webservice_get_site_info")},
                {"moodlewsrestformat", new StringContent("json")}
            });

        var detailedUserInformaionResponse = await MoodleCallAsync<DetailedUserDataResponse[]>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("core_user_get_users_by_field")},
                {"field", new StringContent("id")},
                {"values[0]", new StringContent(generalInformationResponse.Userid.ToString())}
            });

        return new LMSUserDataResponse
        {
            LMSUserName = generalInformationResponse.Username,
            IsAdmin = generalInformationResponse.UserIsSiteAdmin,
            UserId = generalInformationResponse.Userid,
            UserEmail = detailedUserInformaionResponse[0].Email
        };
    }

    public async Task<LMSWorldListResponse> SearchWorldsAsync(string token, string searchString,
        bool limitToEnrolled = false)
    {
        var resp = await MoodleCallAsync<LMSWorldListResponse>(new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("core_course_search_courses")},
            {"criterianame", new StringContent("search")},
            {"criteriavalue", new StringContent(searchString)},
            {"limittoenrolled", new StringContent(limitToEnrolled ? "1" : "0")}
        });

        return resp;
    }

    public async Task<H5PAttempts> GetH5PAttemptsAsync(string token, int h5PActivityId)
    {
        return await MoodleCallAsync<H5PAttempts>(new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("mod_h5pactivity_get_attempts")},
            {"h5pactivityid", new StringContent(h5PActivityId.ToString())}
        });
    }


    private async Task<TDtoType> MoodleCallAsync<TDtoType>(Dictionary<string, HttpContent> wsParams,
        PostToMoodleOptions? options = null)
    {
        wsParams.TryAdd("moodlewsrestformat", new StringContent("json"));

        var content = new MultipartFormDataContent();

        foreach (var item in wsParams) content.Add(item.Value, item.Key);

        var moodleApiResponse = await PostToMoodleAsync(content, options);
        var responseString = await moodleApiResponse.Content.ReadAsStringAsync();

        return ParseResponseFromString<TDtoType>(responseString);
    }

    private async Task<TDtoType> MoodleCallAsync<TDtoType>(MultipartFormDataContent content)
    {
        // Add 'moodlewsrestformat' parameter to request if it's not already set.
        if (!content.Headers.Contains("moodlewsrestformat"))
            content.Add(new StringContent("json"), "moodlewsrestformat");

        var moodleApiResponse = await PostToMoodleAsync(content);
        var responseString = await moodleApiResponse.Content.ReadAsStringAsync();

        return ParseResponseFromString<TDtoType>(responseString);
    }


    private TResponseType ParseResponseFromString<TResponseType>(string responseString)
    {
        ThrowIfMoodleError(responseString);


        return TryRead<TResponseType>(responseString);
    }


    private static TResponse TryRead<TResponse>(string responseString)
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<TResponse>(responseString, options)!;
        }
        catch (Exception e)
        {
            throw new LmsException("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden " + responseString, e);
        }
    }

    private void ThrowIfMoodleError(string responseString)
    {
        MoodleWsErrorResponse wsErrorData = null!;
        try
        {
            wsErrorData = TryRead<MoodleWsErrorResponse>(responseString);
        }
        catch (Exception)
        {
            // ignored
        }

        if (wsErrorData?.ErrorCode != null)
            throw new LmsException("Response from LMS is: " + responseString)
            {
                LmsErrorCode = wsErrorData.ErrorCode
            };
    }

    private async Task<HttpResponseMessage> PostToMoodleAsync(MultipartFormDataContent content,
        PostToMoodleOptions? options = null)
    {
        var url = "";

        try
        {
            options ??= new PostToMoodleOptions();
            url = options.Endpoint switch
            {
                PostToMoodleOptions.Endpoints.Webservice => _configuration["ASPNETCORE_ADLER_MOODLEURL"] +
                                                            "/webservice/rest/server.php",
                PostToMoodleOptions.Endpoints.Login =>
                    _configuration["ASPNETCORE_ADLER_MOODLEURL"] + "/login/token.php",
                _ => url
            };

            return await _client.PostAsync(url, content);
        }
        catch (Exception e)
        {
            throw new LmsException("Die Moodle Web Api ist nicht erreichbar: URL: " + url, e);
        }
    }
}