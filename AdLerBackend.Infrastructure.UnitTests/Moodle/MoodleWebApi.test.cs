using AdLerBackend.Application.Common.Exceptions.LMSAdapter;
using AdLerBackend.Application.Configuration;
using AdLerBackend.Infrastructure.Moodle;
using AdLerBackend.Infrastructure.Moodle.ApiResponses;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AdLerBackend.Infrastructure.UnitTests.Moodle;

public class MoodleWebApiTest
{
    private IOptions<BackendConfig> _configuration;
    private MockHttpMessageHandler _mockHttp = null!;
    private MoodleWebApi _systemUnderTest = null!;

    [SetUp]
    public void SetUp()
    {
        _mockHttp = new MockHttpMessageHandler();
        _configuration = Options.Create(new BackendConfig {MoodleUrl = "http://localhost"});

        _systemUnderTest = new MoodleWebApi(_mockHttp.ToHttpClient(), _configuration);
    }

    [Test]
    public async Task UploadCourseWorldToLMS_Valid()
    {
        // Arrange
        _mockHttp.When("*").Respond("application/json",
            "{\"data\":{\"course_id\":1337}}");

        // Act
        var result = await _systemUnderTest.UploadCourseWorldToLMS("token", new MemoryStream());

        // Assert
        Assert.That(result.CourseLmsId, Is.EqualTo(1337));
    }

    [Test]
    public async Task GetCourseForUser_Valid_GetCoursesForUser()
    {
        // Arrange
        _mockHttp.When("*").Respond("application/json",
            "{\"total\":6,\"courses\":[{\"id\":315,\"fullname\":\"MetrikenTeil1Lernwelt\",\"displayname\":\"MetrikenTeil1Lernwelt\",\"shortname\":\"MetrikenTeil1Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":320,\"fullname\":\"Metriken_Teil2_Lernwelt\",\"displayname\":\"Metriken_Teil2_Lernwelt\",\"shortname\":\"Metriken_Teil2_Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":321,\"fullname\":\"Metriken_Teil3_Lernwelt\",\"displayname\":\"Metriken_Teil3_Lernwelt\",\"shortname\":\"Metriken_Teil3_Lernwelt\",\"categoryid\":10,\"categoryname\":\"PhilippsTesträume\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":329,\"fullname\":\"MetrikenTeil3Lernwelt\",\"displayname\":\"MetrikenTeil3Lernwelt\",\"shortname\":\"MetrikenTeil3Lernwelt\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":330,\"fullname\":\"MetrikenTeil3LernweltMitUuidcopy1\",\"displayname\":\"MetrikenTeil3LernweltMitUuidcopy1\",\"shortname\":\"MetrikenTeil3Lernwelt_1\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":0,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]},{\"id\":286,\"fullname\":\"LernweltMetriken\",\"displayname\":\"LernweltMetriken\",\"shortname\":\"LernweltMetriken\",\"categoryid\":5,\"categoryname\":\"DimisTests\",\"sortorder\":40004,\"summary\":\"\",\"summaryformat\":1,\"summaryfiles\":[],\"overviewfiles\":[],\"showactivitydates\":true,\"showcompletionconditions\":true,\"contacts\":[],\"enrollmentmethods\":[\"guest\"]}],\"warnings\":[]}");

        // Act
        var result = await _systemUnderTest.GetWorldsForUserAsync("token");

        // Assert
        Assert.That(result.Total, Is.EqualTo(6));
    }

    [Test]
    public async Task GetCourseContent_Valid_ReturnCourseContent()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond("application/json",
                "[{\"id\":21022,\"name\":\"General\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":0,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[]},{\"id\":21020,\"name\":\"Tile1\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":1,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":685,\"url\":\"https://moodle.cluuub.xyz/mod/h5pactivity/view.php?id=685\",\"name\":\"Element_1\",\"instance\":323,\"contextid\":1146,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/h5pactivity/1651754230/icon\",\"modname\":\"h5pactivity\",\"modplural\":\"H5P\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"\\\"\\\"\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[]}]},{\"id\":21021,\"name\":\"Tile2\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":2,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":686,\"url\":\"https://moodle.cluuub.xyz/mod/h5pactivity/view.php?id=686\",\"name\":\"Element_2\",\"instance\":324,\"contextid\":1147,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/h5pactivity/1651754230/icon\",\"modname\":\"h5pactivity\",\"modplural\":\"H5P\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"\\\"\\\"\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[]}]},{\"id\":21019,\"name\":\"Tile3\",\"visible\":1,\"summary\":\"\",\"summaryformat\":1,\"section\":3,\"hiddenbynumsections\":0,\"uservisible\":true,\"modules\":[{\"id\":684,\"url\":\"https://moodle.cluuub.xyz/mod/resource/view.php?id=684\",\"name\":\"DSL_Document\",\"instance\":112,\"contextid\":1145,\"visible\":1,\"uservisible\":true,\"visibleoncoursepage\":1,\"modicon\":\"https://moodle.cluuub.xyz/theme/image.php/boost/core/1651754230/f/text-24\",\"modname\":\"resource\",\"modplural\":\"Files\",\"availability\":null,\"indent\":0,\"onclick\":\"\",\"afterlink\":null,\"customdata\":\"{\\\"displayoptions\\\":\\\"\\\",\\\"display\\\":5}\",\"noviewlink\":false,\"completion\":1,\"completiondata\":{\"state\":0,\"timecompleted\":0,\"overrideby\":null,\"valueused\":false,\"hascompletion\":true,\"isautomatic\":false,\"istrackeduser\":false,\"uservisible\":true,\"details\":[]},\"dates\":[],\"contents\":[{\"type\":\"file\",\"filename\":\"DSL_Document\",\"filepath\":\"/\",\"filesize\":942,\"fileurl\":\"https://moodle.cluuub.xyz/webservice/pluginfile.php/1145/mod_resource/content/0/DSL_Document?forcedownload=1\",\"timecreated\":1659961413,\"timemodified\":1659961413,\"sortorder\":0,\"mimetype\":\"text/plain\",\"isexternalfile\":false,\"userid\":3,\"author\":null,\"license\":\"unknown\"}],\"contentsinfo\":{\"filescount\":1,\"filessize\":942,\"lastmodified\":1659961413,\"mimetypes\":[\"text/plain\"],\"repositorytype\":\"\"}}]}]"
            );

        // Act
        var result = await _systemUnderTest.GetWorldContentAsync("token", 1);

        // Assert
        Assert.That(result, Has.Length.EqualTo(4));
    }

    [Test]
    public async Task GetMoodleToken_ValidResponse_ReturnsToken()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json", "{\"token\":\"testToken\"}");

        // Act
        var result = await _systemUnderTest.GetLMSUserTokenAsync("moodleUser", "moodlePassword");

        // Assert
        Assert.That(result.LMSToken, Is.EqualTo("testToken"));
    }

    [Test]
    public Task GetMoodleToken_InvalidResponse_ReturnsException()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "{\"error\":\"Invalidlogin,pleasetryagain\",\"errorcode\":\"invalidlogin\",\"stacktrace\":null,\"debuginfo\":null,\"reproductionlink\":null}");

        Assert.ThrowsAsync<LmsException>(() =>
            _systemUnderTest.GetLMSUserTokenAsync("moodleUser", "moodlePassword"));
        return Task.CompletedTask;
    }

    [Test]
    public Task MoodleAPI_WSNotAvaliblale_ReturnsException()
    {
        // Arrange
        _mockHttp.When("*")
            .Throw(new HttpRequestException());

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetLMSUserTokenAsync("moodleUser", "moodlePassword"));
        return Task.CompletedTask;
    }

    [Test]
    public Task MoodleAPI_WSNotReadAble_Throws()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "<blablabla>");

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetLMSUserTokenAsync("moodleUser", "moodlePassword"));

        // check exception message
        Assert.That(exception!.Message,
            Contains.Substring("Das Ergebnis der Moodle Web Api konnte nicht gelesen werden"));
        return Task.CompletedTask;
    }

    [Test]
    public async Task GetMoodleUserData_ValidResponse_ReturnsUserData()
    {
        // Arrange
        var firstResponse = new
        {
            Userid = 1,
            Userissiteadmin = true,
            Username = "testUser"
        };
        var secondResponse = new[] {new {email = "test"}}.ToList();

        _mockHttp.When("*")
            .With(request => MatchesFormData(request, "core_webservice_get_site_info"))
            .Respond("application/json", JsonSerializer.Serialize(firstResponse));

        _mockHttp.When("*")
            .With(request => MatchesFormData(request, "core_user_get_users_by_field"))
            .Respond("application/json", JsonSerializer.Serialize(secondResponse));

        // Act
        var result = await _systemUnderTest.GetLMSUserDataAsync("moodleToken");

        // Assert
        Assert.That(result.UserId, Is.EqualTo(1));
    }

    [Test]
    public Task GetMoodleUserData_InvalidResponseWrongToken_ThrowsCorrectException()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json",
                "{\"exception\":\"moodle_exception\",\"errorcode\":\"invalidtoken\",\"message\":\"Invalidtoken-tokennotfound\"}");

        var exception = Assert.ThrowsAsync<LmsException>(async () =>
            await _systemUnderTest.GetLMSUserDataAsync("moodleToken"));

        // check exception message
        Assert.That(exception!.LmsErrorCode, Is.EqualTo("invalidtoken"));
        return Task.CompletedTask;
    }

    [Test]
    public async Task ProcessXApi_Valid_ReturnsTrue()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json", "{\"data\":[{\"module_id\":209,\"score\":17}]}");

        // Act
        var result = await _systemUnderTest.ProcessXApiViaPlugin("moodleToken", "testXApi");

        // Assert
        Assert.That(result, Is.EqualTo(true));
    }

    [Test]
    public async Task ProcessXApi_InvalidButParsing_ReturnsFalse()
    {
        // Arrange
        _mockHttp.When("*")
            .Respond(
                "application/json", "{\"data\":[{\"module_id\":209,\"score\":0}]}");

        // Act
        var result = await _systemUnderTest.ProcessXApiViaPlugin("moodleToken", "testXApi");

        // Assert
        Assert.That(result, Is.EqualTo(false));
    }

    [Test]
    public async Task GetElementScoreFromPlugin_ReturnsFalse_WhenScoreIsZero()
    {
        // Arrange
        var expectedResponse = new ResponseWithDataArray<PluginElementScoreData>
        {
            Data = new List<PluginElementScoreData>
            {
                new()
                {
                    Score = 0,
                    Module_id = 123
                }
            }
        };

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(expectedResponse));

        // Act
        var result = await _systemUnderTest.GetElementScoreFromPlugin("token", 123);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public async Task ScoreGenericElementViaPlugin_ReturnsTrue_WhenScoreIsGreaterThanZero()
    {
        var expectedResponse = new ResponseWithDataArray<PluginElementScoreData>
        {
            Data = new List<PluginElementScoreData>
            {
                new()
                {
                    Score = 1,
                    Module_id = 123
                }
            }
        };

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(expectedResponse));

        // Act
        var result = await _systemUnderTest.ScoreGenericElementViaPlugin("token", 123);

        // Assert
        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetCourseStatusViaPlugin_ReturnsValidResponse_WhenResponseIsValid()
    {
        var webResponse = new ResponseWithDataArray<PluginElementScoreData>
        {
            Data = new List<PluginElementScoreData>
            {
                new()
                {
                    Score = 1,
                    Module_id = 1
                }
            }
        };

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(webResponse));

        // Act
        var result = await _systemUnderTest.GetCourseStatusViaPlugin("token", 123);

        // Assert
        Assert.That(result.ElementScores.Count, Is.EqualTo(1));
        Assert.That(result.ElementScores[0].HasScored, Is.EqualTo(true));
        Assert.That(result.ElementScores[0].ModuleId, Is.EqualTo(1));
    }

    private bool MatchesFormData(HttpRequestMessage request, string wsfunction)
    {
        if (request.Content is MultipartFormDataContent formContent)
        {
            var contentString = formContent.ReadAsStringAsync().Result;

            return contentString.Contains(wsfunction);
        }

        return false;
    }


    [Test]
    public async Task GetLmsElementIdsByUuidsAsync_ReturnsExpectedElements()
    {
        // Arrange
        var webResponse = new ResponseWithDataArray<PluginUUIDResponse>
        {
            Data = new List<PluginUUIDResponse>
            {
                new()
                {
                    ContextId = 1,
                    Uuid = "UUID1",
                    CourseId = "eins",
                    MoodleId = 1,
                    ElementType = "cm"
                },
                new()
                {
                    ContextId = 2,
                    Uuid = "UUID2",
                    CourseId = "zwei",
                    MoodleId = 2,
                    ElementType = "cm"
                }
            }
        };

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(webResponse));

        // Act
        var result =
            await _systemUnderTest.GetLmsElementIdsByUuidsAsync("token", 1, new List<string> {"UUID1", "UUID2"});

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.First().Uuid, Is.EqualTo("UUID1"));
    }

    // Course Deletion
    [Test]
    public async Task DeleteCourseAsync_DoesNotThrow_WhenResponseIsValid()
    {
        // Arrange
        var webResponse = new ResponseWithDataArray<CourseDeletionWarningsResponse>();

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(webResponse));

        // Act and assert that it does not throw
        await _systemUnderTest.DeleteCourseAsync("token", 1);
    }

    [Test]
    public async Task DeleteCourseAsync_Throws_WhenResponseIsInvalid()
    {
        // Arrange
        var webResponse = new ResponseWithDataArray<CourseDeletionWarningsResponse>
        {
            Data = new List<CourseDeletionWarningsResponse>
            {
                new()
                {
                    Warningcode = "xxxxx"
                }
            }
        };

        _mockHttp.When("*").Respond("application/json",
            JsonConvert.SerializeObject(webResponse));

        // Act and assert that it does not throw
        Assert.ThrowsAsync<LmsException>(async () => await _systemUnderTest.DeleteCourseAsync("token", 1));
    }

    [Test]
    public async Task GetAdaptivityTaskDetailsAsync_Valid_ReturnsTasks()
    {
        // Arrange
        var webResponse =
            "{\n    \"data\": {\n        \"Tasks\": [\n            {\n                \"uuid\": \"298a7c8b-f6a6-41a7-b54f-065c70dc47c0\",\n                \"status\": \"Correct\"\n            },\n            {\n                \"uuid\": \"febcc2e5-c8b5-48c7-b1b7-e729e2bb12c3\",\n                \"status\": \"Incorrect\"\n            },\n            {\n                \"uuid\": \"687d3191-dc59-4142-a7cb-957049e50fcf \",\n                \"status\": \"NotAttempted\"\n            },\n            {\n                \"uuid\": \"8b2d1cc2-e567-4558-aae5-55239deb3494\",\n                \"status\": \"Correct\"\n            }\n        ]\n    }\n}";


        _mockHttp.When("*").Respond("application/json", webResponse);

        // Act
        var result = await _systemUnderTest.GetAdaptivityTaskDetailsAsync("token", 1);

        // Assert
        Assert.That(result.Count, Is.EqualTo(4));
    }

    [Test]
    public async Task GetAdaptivityElementDetailsAsync_Valid_ReturnsQuestionDetails()
    {
        // Arrange
        var webResponse =
            "{\n    \"data\": {\n        \"questions\": [\n            {\n                \"uuid\": \"298a7c8b-f6a6-41a7-b54f-065c70dc47c0\",\n                \"status\": \"correct\",\n                \"answers\": \"[{\\\"checked\\\":false,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":true},{\\\"checked\\\":true,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":true}]\"\n            },\n            {\n                \"uuid\": \"febcc2e5-c8b5-48c7-b1b7-e729e2bb12c3\",\n                \"status\": \"incorrect\",\n                \"answers\": \"[{\\\"checked\\\":false,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":false},{\\\"checked\\\":true,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":false}]\"\n            },\n            {\n                \"uuid\": \"687d3191-dc59-4142-a7cb-957049e50fcf \",\n                \"status\": \"notAttempted\",\n                \"answers\": null\n            },\n            {\n                \"uuid\": \"8b2d1cc2-e567-4558-aae5-55239deb3494\",\n                \"status\": \"correct\",\n                \"answers\": \"[{\\\"checked\\\":false,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":true},{\\\"checked\\\":true,\\\"answer_correct\\\":true},{\\\"checked\\\":false,\\\"answer_correct\\\":true}]\"\n            }\n        ]\n    }\n}";

        _mockHttp.When("*").Respond("application/json", webResponse);

        // Act
        var result = await _systemUnderTest.GetAdaptivityElementDetailsAsync("token", 1);

        // Assert
        Assert.That(result.Count, Is.EqualTo(4));
        Assert.That(result.First().Answers.Count, Is.EqualTo(4));
    }
}