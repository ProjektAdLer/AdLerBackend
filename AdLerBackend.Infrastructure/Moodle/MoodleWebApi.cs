using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using Microsoft.Extensions.Configuration;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedAutoPropertyAccessor.Local

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
        var resp = await MoodleCallAsync<UserTokenResponse>(new Dictionary<string, string>
        {
            {"username", userName},
            {"password", password},
            {"service", "moodle_mobile_app"}
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
        var resp = await MoodleCallAsync<WorldContent[]>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_get_contents"},
            {"courseid", worldId.ToString()}
        });
        return resp;
    }

    public async Task<LMSWorldListResponse> GetWorldsForUserAsync(string token)
    {
        return await MoodleCallAsync<LMSWorldListResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_search_courses"},
            {"criterianame", "search"},
            {"criteriavalue", ""},
            {"limittoenrolled", "1"}
        });
    }

    public async Task<bool> IsLMSAdminAsync(string token)
    {
        var userData = await GetLMSUserDataAsync(token);
        return userData.IsAdmin;
    }

    public async Task<int> UploadCourseWorldToLMS(string token, Stream backupFileStream)
    {
        // Encode the Stream in Base64
        var base64String = _moodleUtils.ConvertFileStreamToBase64(backupFileStream);

        var response = await MoodleCallAsync<UploadWorldPluginResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "local_adler_upload_course"},
            {"mbz", base64String}
        });

        return response.Data.Course_Id;
    }

    public async Task<bool> GetElementScoreFromPlugin(string token, int elementId)
    {
        var response = await MoodleCallAsync<PluginElementScore>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "local_adler_score_get_element_scores"},
            {"module_ids[0]", elementId.ToString()}
        });

        // Todo replace with the actual score
        return response.data[0].score > 0;
    }

    public async Task<bool> ScoreGenericElementViaPlugin(string token, int elementId)
    {
        var response = await MoodleCallAsync<PluginElementScore>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "local_adler_score_primitive_learning_element"},
            {"module_id", elementId.ToString()},
            {"is_completed", "1"}
        });

        return response.data[0].score > 0;
    }

    public async Task<bool> ProcessXApiViaPlugin(string token, string statement)
    {
        var response = await MoodleCallAsync<PluginElementScore>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "local_adler_score_h5p_learning_element"},
            {"xapi", "[" + statement + "]"}
        });

        return response.data[0].score > 0;
    }

    public async Task<LmsCourseStatusResponse> GetCourseStatusViaPlugin(string token, int courseId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, string>
            {
                {"wstoken", token},
                {"moodlewsrestformat", "json"},
                {"wsfunction", "local_adler_score_get_course_scores"},
                {"course_id", courseId.ToString()}
            });

        var courseStatus = new LmsCourseStatusResponse
        {
            ElementScores = response.Data
                .Where(x => x.score != null) // Filter out non-AdLer Courses
                .Select(x => new LmsElementStatus // Map to LMS Element Status
                {
                    ModuleId = x.module_id,
                    HasScored = x.score > 0
                })
                .ToList()
        };

        return courseStatus;
    }


    public virtual async Task<LMSUserDataResponse> GetLMSUserDataAsync(string token)
    {
        var generalInformationResponse = await MoodleCallAsync<GeneralUserDataResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"wsfunction", "core_webservice_get_site_info"},
            {"moodlewsrestformat", "json"}
        });

        var detailedUserInformaionResponse = await MoodleCallAsync<DetailedUserDataResponse[]>(
            new Dictionary<string, string>
            {
                {"wstoken", token},
                {"wsfunction", "core_user_get_users_by_field"},
                {"moodlewsrestformat", "json"},
                {"field", "id"},
                {"values[0]", generalInformationResponse.Userid.ToString()}
            });

        return new LMSUserDataResponse
        {
            LMSUserName = generalInformationResponse.Username,
            IsAdmin = generalInformationResponse.Userissiteadmin,
            UserId = generalInformationResponse.Userid,
            UserEmail = detailedUserInformaionResponse[0].Email
        };
    }

    public async Task<LMSWorldListResponse> SearchWorldsAsync(string token, string searchString,
        bool limitToEnrolled = false)
    {
        var resp = await MoodleCallAsync<LMSWorldListResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "core_course_search_courses"},
            {"criterianame", "search"},
            {"criteriavalue", searchString},
            {"limittoenrolled", limitToEnrolled ? "1" : "0"}
        });

        return resp;
    }

    public async Task<H5PAttempts> GetH5PAttemptsAsync(string token, int h5PActivityId)
    {
        return await MoodleCallAsync<H5PAttempts>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "mod_h5pactivity_get_attempts"},
            {"h5pactivityid", h5PActivityId.ToString()}
        });
    }

    public async Task<bool> ScoreGenericElement(string token, int elementId)
    {
        var response = await MoodleCallAsync<ScoreGenericLearningElementResponse>(new Dictionary<string, string>
        {
            {"wstoken", token},
            {"moodlewsrestformat", "json"},
            {"wsfunction", "format_tiles_update_activity_completion_status_manually"},
            {"cmid", elementId.ToString()},
            {"completed", "1"}
        });

        return response.Status;
    }


    private async Task<TDtoType> MoodleCallAsync<TDtoType>(Dictionary<string, string> wsParams,
        PostToMoodleOptions? options = null)
    {
        var moodleApiResponse = await PostToMoodleAsync(wsParams, options);
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
            throw new LmsException("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden", e);
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

        if (wsErrorData?.Errorcode != null)
            throw new LmsException
            {
                LmsErrorCode = wsErrorData.Errorcode
            };
    }

    private async Task<HttpResponseMessage> PostToMoodleAsync(Dictionary<string, string> wsParams,
        PostToMoodleOptions? options = null)
    {
        var url = "";

        try
        {
            options ??= new PostToMoodleOptions();
            switch (options.Endpoint)
            {
                case PostToMoodleOptions.Endpoints.Webservice:
                    url = _configuration["moodleUrl"] + "/webservice/rest/server.php";
                    break;
                case PostToMoodleOptions.Endpoints.Login:
                    url = _configuration["moodleUrl"] + "/login/token.php";
                    break;
            }

            return await _client.PostAsync(url,
                new FormUrlEncodedContent(wsParams));
        }
        catch (Exception e)
        {
            throw new LmsException("Die Moodle Web Api ist nicht erreichbar: URL: " + url, e);
        }
    }
#pragma warning disable CS8618
    public class PluginElementScoreData
    {
        public int module_id { get; set; }
        public int? score { get; set; }
    }

    public class PluginElementScore
    {
        public List<PluginElementScoreData> data { get; set; }
    }

    private class UserTokenResponse
    {
        public string Token { get; set; }
    }

    private class GeneralUserDataResponse
    {
        public string Username { get; set; }
        public bool Userissiteadmin { get; set; }
        public int Userid { get; set; }
    }

    private class DetailedUserDataResponse
    {
        public string Email { get; set; }
    }

    public class UploadWorldPluginResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public int Course_Id { get; set; }
    }

    private class MoodleWsErrorResponse
    {
        public string Errorcode { get; set; }
    }

    private class PostToMoodleOptions
    {
        public enum Endpoints
        {
            Webservice,
            Login
        }

        public Endpoints Endpoint { get; set; } = Endpoints.Webservice;
    }


    public class ScoreGenericLearningElementResponse
    {
        public bool Status { get; set; }
    }

    public class ResponseWithDataArray<T>
    {
        public IList<T> Data { get; set; }
    }
#pragma warning restore CS8618
}