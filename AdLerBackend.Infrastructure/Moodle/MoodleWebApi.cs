﻿using System.Text.Json;
using AdLerBackend.Application.Common.DTOs;
using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Common.Interfaces;
using AdLerBackend.Application.Common.Responses.LMSAdapter;
using AdLerBackend.Application.Common.Responses.LMSAdapter.Adaptivity;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure.Moodle.ApiResponses;
using AdLerBackend.Infrastructure.Moodle.ApiResponses.Common;
using AdLerBackend.Infrastructure.Moodle.ApiResponses.PluginResponses;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

#pragma warning disable CS8524

namespace AdLerBackend.Infrastructure.Moodle;

public class MoodleWebApi : ILMS
{
    private readonly HttpClient _client;
    private readonly BackendConfig _configuration;
    private readonly ILogger<MoodleWebApi> _logger;

    public MoodleWebApi(HttpClient client, IOptions<BackendConfig> configuration, ILogger<MoodleWebApi> logger)
    {
        _client = client;
        _configuration = configuration.Value;
        _logger = logger;

        // set timeout to 600 seconds
        _client.Timeout = TimeSpan.FromSeconds(600);
    }

    public async Task<LMSUserTokenResponse> GetLMSUserTokenAsync(string userName, string password)
    {
        var resp = await MoodleCallAsync<UserTokenResponse>(new Dictionary<string, HttpContent>
        {
            {"username", new StringContent(userName)},
            {"password", new StringContent(password)},
            {"service", new StringContent("adler_services")}
        }, new PostToMoodleOptions
        {
            Endpoint = PostToMoodleOptions.Endpoints.Login
        });

        return new LMSUserTokenResponse
        {
            LMSToken = resp.Token
        };
    }

    public async Task<LMSUserTokenResponse> GetLMSAdminTokenAsync(string userName, string password)
    {
        var resp = await MoodleCallAsync<UserTokenResponse>(new Dictionary<string, HttpContent>
        {
            {"wsfunction", new StringContent("local_adler_site_admin_login")},
            {"wsusername", new StringContent(userName)},
            {"wspassword", new StringContent(password)}
        }, new PostToMoodleOptions
        {
            Endpoint = PostToMoodleOptions.Endpoints.AdminLogin
        });

        return new LMSUserTokenResponse
        {
            LMSToken = resp.Token
        };
    }

    public async Task<LMSWorldContentResponse[]> GetWorldContentAsync(string token, int worldId)
    {
        var resp = await MoodleCallAsync<LMSWorldContentResponse[]>(new Dictionary<string, HttpContent>
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


    public async Task DeleteCourseViaPluginAsync(string token, int courseId)
    {
        var warnings = await MoodleCallAsync<ResponseWithDataArray<CourseDeletionWarningsResponse>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("core_course_delete_courses")},
                {"courseids[0]", new StringContent(courseId.ToString())}
            });

        // if any warnings are returned, throw an exception warnings can also be null
        if (warnings?.Data?.Count > 0)
            throw new LmsException("Course could not be deleted because of the following warnings: " +
                                   JsonSerializer.Serialize(warnings.Data));
    }

    public async Task<IEnumerable<LMSAdaptivityQuestionStateResponse>> GetAdaptivityElementDetailsViaPluginAsync(string token,
        int elementId)
    {
        var rawResponse = await MoodleCallAsync<ResponseWithData<PluginAdaptivityQuestionsDetailResponse>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("mod_adleradaptivity_get_question_details")},
                {"module[module_id]", new StringContent(elementId.ToString())}
            });

        var response = rawResponse.Data.Questions.Select(x => new LMSAdaptivityQuestionStateResponse
        {
            Uuid = Guid.Parse(x.Uuid),
            Status = Enum.Parse<AdaptivityStates>(x.Status, true),
            Answers = x.Answers != null
                ? JsonSerializer.Deserialize<IList<LMSAdaptivityQuestionStateResponse.LMSAdaptivityAnswers>>(
                    x.Answers, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    })
                : null
        }).ToList();

        return response;
    }

    public async Task<IEnumerable<LMSAdaptivityTaskStateResponse>> GetAdaptivityTaskDetailsViaPluginAsync(string token,
        int elementId)
    {
        var rawResponse = await MoodleCallAsync<ResponseWithData<PluginAdaptivityTasksResponse>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("mod_adleradaptivity_get_task_details")},
                {"module[module_id]", new StringContent(elementId.ToString())}
            });

        var response = rawResponse.Data.Tasks.Select(x => new LMSAdaptivityTaskStateResponse
        {
            Uuid = Guid.Parse(x.Uuid),
            State = Enum.Parse<AdaptivityStates>(x.Status, true)
        }).ToList();

        return response;
    }

    public async Task<LMSCourseCreationResponse> UploadCourseWorldToLmsViaPluginAsync(string token, Stream backupFileStream)
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

    public async Task<bool> GetElementScoreViaPluginAsync(string token, int elementId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_get_element_scores")},
                {"module_ids[0]", new StringContent(elementId.ToString())}
            });

        return response.Data[0].Completed;
    }

    public async Task<bool> ScoreGenericElementViaPluginAsync(string token, int elementId)
    {
        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_trigger_event_cm_viewed")},
                {"module_id", new StringContent(elementId.ToString())}
            });

        return response.Data[0].Completed;
    }

    public async Task<bool> ProcessXApiViaPluginAsync(string token, string statement)
    {
        // // First trigger the "viewed" event
        // var score = await ScoreGenericElementViaPluginAsync(token, 0);

        var response = await MoodleCallAsync<ResponseWithDataArray<PluginElementScoreData>>(
            new Dictionary<string, HttpContent>
            {
                {"wstoken", new StringContent(token)},
                {"wsfunction", new StringContent("local_adler_score_h5p_learning_element")},
                {"xapi", new StringContent("[" + statement + "]")}
            });

        return response.Data[0].Completed;
    }

    public async Task<LmsCourseStatusResponse> GetCourseStatusViaPluginAsync(string token, int courseId)
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
                    HasScored = x.Completed,
                    Score = x.Score ?? 0
                })
                .ToList()
        };

        return courseStatus;
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
            UserId = generalInformationResponse.Userid,
            UserEmail = detailedUserInformaionResponse[0].Email
        };
    }

    public async Task<IEnumerable<LmsUuidResponse>> GetLmsElementIdsByUuidsViaPluginAsync(string token, int courseInstanceId,
        IEnumerable<Guid> uuids)
    {
        var wsParams = new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("local_adler_get_element_ids_by_uuids")}
        };

        for (var i = 0; i < uuids.Count(); i++)
        {
            wsParams.Add($"elements[{i}][course_id]", new StringContent(courseInstanceId.ToString()));
            wsParams.Add($"elements[{i}][element_type]", new StringContent("cm"));
            wsParams.Add($"elements[{i}][uuid]", new StringContent(uuids.ElementAt(i).ToString()));
        }

        var ret = await MoodleCallAsync<ResponseWithDataArray<PluginUUIDResponse>>(wsParams);

        return ret.Data.Select(x => new LmsUuidResponse
        {
            Uuid = x.Uuid,
            LmsId = x.MoodleId,
            LmsContextId = x.ContextId
        });
    }

    public async Task<AdaptivityModuleStateResponseAfterAnswer> AnswerAdaptivityQuestionsViaPluginAsync(string token,
        int elementId, IEnumerable<AdaptivityAnsweredQuestionTo> answeredQuestions)
    {
        var wsParams = new Dictionary<string, HttpContent>
        {
            {"wstoken", new StringContent(token)},
            {"wsfunction", new StringContent("mod_adleradaptivity_answer_questions")},
            {"module[module_id]", new StringContent(elementId.ToString())}
        };

        for (var i = 0; i < answeredQuestions.Count(); i++)
        {
            wsParams.Add($"questions[{i}][uuid]", new StringContent(answeredQuestions.ElementAt(i).Uuid));
            wsParams.Add($"questions[{i}][answer]", new StringContent(answeredQuestions.ElementAt(i).Answer));
        }

        var result = await MoodleCallAsync<ResponseWithData<PluginAdaptivityAnsweredQuestionResponse>>(wsParams);

        return new AdaptivityModuleStateResponseAfterAnswer
        {
            Questions = result.Data.Questions.Select(x => new LMSAdaptivityQuestionStateResponse
            {
                Uuid = Guid.Parse(x.Uuid),
                Status = Enum.Parse<AdaptivityStates>(x.Status, true),
                Answers = x.Answers != null
                    ? JsonSerializer.Deserialize<IList<LMSAdaptivityQuestionStateResponse.LMSAdaptivityAnswers>>(
                        x.Answers, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        })
                    : null
            }).ToList(),
            Tasks = result.Data.Tasks.Select(x => new LMSAdaptivityTaskStateResponse
            {
                Uuid = Guid.Parse(x.Uuid),
                State = Enum.Parse<AdaptivityStates>(x.Status, true)
            }).ToList(),
            State = Enum.Parse<AdaptivityStates>(result.Data.Module.Status, true)
        };
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
            throw new LmsException(
                "Das Ergebnis der Moodle Web Api konnte nicht gelesen werden. Response string is: " + responseString,
                e);
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
                PostToMoodleOptions.Endpoints.Webservice => _configuration.MoodleUrl + "/webservice/rest/server.php",
                PostToMoodleOptions.Endpoints.Login => _configuration.MoodleUrl + "/login/token.php",
                PostToMoodleOptions.Endpoints.AdminLogin => _configuration.MoodleUrl +
                                                            "/webservice/rest/simpleserver.php"
            };


            _logger.LogInformation("Request to Moodle: {Url}", url);


            return await _client.PostAsync(url, content);
        }
        catch (Exception e)
        {
            throw new LmsException("Die Moodle Web Api ist nicht erreichbar: URL: " + url, e);
        }
    }
}