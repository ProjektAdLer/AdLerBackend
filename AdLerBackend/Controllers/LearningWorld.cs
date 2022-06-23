using Microsoft.AspNetCore.Mvc;

namespace AdLerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LearningWorld : ControllerBase
    {
        private ILogger<LearningWorld> Logger { get; }

        public LearningWorld(ILogger<LearningWorld> logger)
        {
            Logger = logger;
        }
        private static readonly HttpClient Client = new();
        [HttpPost]
        public async Task<DslFile> GetWorldFile(GetLearningWorldDto data)
        {
            Logger.LogTrace("GetWorldFile - wsToken: {} courseName: {}", data.wsToken, data.courseName);
            const string moodleDomain = "https://moodle.cluuub.xyz";
            const string webservicePath = moodleDomain + "/webservice";
            // Get Course by Name
            var getCourseParams = new Dictionary<string, string>
             {
                    { "wstoken", data.wsToken },
                    { "moodlewsrestformat", "json" },
                    { "wsfunction", "core_course_search_courses" },
                    { "criterianame", "search" },
                    { "criteriavalue", data.courseName }
            };

            var coursesResponse = await Client.PostAsync($"{webservicePath}/rest/server.php",
                            new FormUrlEncodedContent(getCourseParams));
            Logger.LogTrace("coursesResponse: Status {}", coursesResponse.StatusCode);

            var searchedCourses = coursesResponse.Content.ReadFromJsonAsync<SearchCoursesResponseDto>().Result;
            if (searchedCourses == null) 
                throw new Exception("Couldn't parse moodle course response into SearchResponse object.");
            // Get File in Course

            var getCourseContentParams = new Dictionary<string, string>
            {
                    { "wstoken", data.wsToken },
                    { "moodlewsrestformat", "json" },
                    { "wsfunction", "core_course_get_contents" },
                    { "courseid", searchedCourses.courses.FirstOrDefault()?.id.ToString() ??
                                  throw new Exception("Couldn't get course id from moodle response.") }
            };

            var courseContentResponse = await Client.PostAsync($"{webservicePath}/rest/server.php",
                            new FormUrlEncodedContent(getCourseContentParams));

            var courseContents = await courseContentResponse.Content.ReadFromJsonAsync<CourseContents[]>();

            var filtered =  courseContents?.First(c => c.modules.FirstOrDefault()?.name == "DSL_Document");

            var contextId = filtered?.modules.FirstOrDefault()?.contextid;
            if (contextId == null)
                throw new Exception("Couldn't get a context id for the DSL file from the course contents response.");
            
            // Download File
            var response =
                await Client.GetAsync(
                    $"{webservicePath}/pluginfile.php/{contextId}/mod_resource/content/0/DSL_Document?token={data.wsToken}");

            var dslFile = response.Content.ReadFromJsonAsync<DslFile>().Result;
            if (dslFile == null)
                throw new Exception("Couldn't parse response for requested DSL file into object.");

            // Fill in DSL with H5P Data

            var getH5PParams = new Dictionary<string, string>
            {
                    { "wstoken", data.wsToken },
                    { "moodlewsrestformat", "json" },
                    { "wsfunction", "mod_h5pactivity_get_h5pactivities_by_courses" },
                    { "courseids[0]", searchedCourses.courses.FirstOrDefault()?.id.ToString() ??
                                      throw new Exception("Couldn't get id of first course in course response.")}
            };

            var h5PResponse = await Client.PostAsync($"{webservicePath}/rest/server.php",
                            new FormUrlEncodedContent(getH5PParams));

            var h5PResponseData = await h5PResponse.Content.ReadFromJsonAsync<H5PResponseDto>();
            if (h5PResponseData == null)
            {
                throw new Exception("Couldn't parse h5p response data into DTO object.");
            }

            foreach (var e in dslFile.learningWorld.learningElements)
            {
                if (e.elementType != "h5p")
                    continue;
                
                var id = h5PResponseData.h5Pactivities.First(h => h.name == e.identifier.value).context;
                var fileName = h5PResponseData.h5Pactivities.First(h => h.name == e.identifier.value).package.FirstOrDefault()?.filename;
                if (fileName == null)
                    throw new Exception($"Couldn't get filename for h5p element of id {id} in h5p response data");

                var ctxIdMetaData = new MetaData
                {
                    key = "h5pContextId",
                    value = id.ToString()
                };
                var fileNameMetaData = new MetaData
                {
                    key = "h5pFileName",
                    value = fileName
                };
                
                e.metaData ??= new List<MetaData>();
                e.metaData.Add(ctxIdMetaData);
                e.metaData.Add(fileNameMetaData);
            }


            //dSLFile.learningWorld.learningElements[0].metaData ??= new List<MetaData>();

            //dSLFile.learningWorld.learningElements[0].metaData.Add(
            //    new MetaData
            //    {
            //        key = "h5pContextId",
            //        value = "test"
            //    });



            return dslFile;
        }
    }
}

public class GetLearningWorldDto
{
    public string wsToken { get; set; }
    public string courseName { get; set; }
}


public class Course
{
    public int id { get; set; }
    public string fullname { get; set; }
    public string displayname { get; set; }
    public string shortname { get; set; }
    public int categoryid { get; set; }
    public string categoryname { get; set; }
    public int sortorder { get; set; }
    public string summary { get; set; }
    public int summaryformat { get; set; }
    public List<object> summaryfiles { get; set; }
    public List<object> overviewfiles { get; set; }
    public bool showactivitydates { get; set; }
    public bool showcompletionconditions { get; set; }
    public List<object> contacts { get; set; }
    public List<string> enrollmentmethods { get; set; }
}

public class SearchCoursesResponseDto
{
    public int total { get; set; }
    public List<Course> courses { get; set; }
    public List<object> warnings { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);
public class Completiondata
{
    public int state { get; set; }
    public int timecompleted { get; set; }
    public object overrideby { get; set; }
    public bool valueused { get; set; }
    public bool hascompletion { get; set; }
    public bool isautomatic { get; set; }
    public bool istrackeduser { get; set; }
    public bool uservisible { get; set; }
    public List<object> details { get; set; }
}

public class Content
{
    public string type { get; set; }
    public string filename { get; set; }
    public string filepath { get; set; }
    public int filesize { get; set; }
    public string fileurl { get; set; }
    public int timecreated { get; set; }
    public int timemodified { get; set; }
    public int sortorder { get; set; }
    public string mimetype { get; set; }
    public bool isexternalfile { get; set; }
    public int userid { get; set; }
    public object author { get; set; }
    public string license { get; set; }
}

public class Contentsinfo
{
    public int filescount { get; set; }
    public int filessize { get; set; }
    public int lastmodified { get; set; }
    public List<string> mimetypes { get; set; }
    public string repositorytype { get; set; }
}

public class Module
{
    public int id { get; set; }
    public string url { get; set; }
    public string name { get; set; }
    public int instance { get; set; }
    public int contextid { get; set; }
    public int visible { get; set; }
    public bool uservisible { get; set; }
    public int visibleoncoursepage { get; set; }
    public string modicon { get; set; }
    public string modname { get; set; }
    public string modplural { get; set; }
    public int indent { get; set; }
    public string onclick { get; set; }
    public object afterlink { get; set; }
    public string customdata { get; set; }
    public bool noviewlink { get; set; }
    public int completion { get; set; }
    public List<object> dates { get; set; }
    public Completiondata completiondata { get; set; }
    public List<Content> contents { get; set; }
    public Contentsinfo contentsinfo { get; set; }
}

public class CourseContents
{
    public int id { get; set; }
    public string name { get; set; }
    public int visible { get; set; }
    public string summary { get; set; }
    public int summaryformat { get; set; }
    public int section { get; set; }
    public int hiddenbynumsections { get; set; }
    public bool uservisible { get; set; }
    public List<Module> modules { get; set; }
}

public class CourseContentsResponseDto
{
    public List<CourseContents> courses { get; set; }
}
public class Identifier
{
    public string type { get; set; }
    public string value { get; set; }
}

public class MetaData
{
    public string key { get; set; }
    public string value { get; set; }
}

public class LearningElement
{
    public int id { get; set; }
    public Identifier identifier { get; set; }
    public string elementType { get; set; }
    public object learningElementValue { get; set; }
    public object requirements { get; set; }
    public List<MetaData>? metaData { get; set; }
}

public class LearningSpace
{
    public int spaceId { get; set; }
    public string learningSpaceName { get; set; }
    public Identifier identifier { get; set; }
    public List<int> learningSpaceContent { get; set; }
    public object requirements { get; set; }
}

public class LearningWorld
{
    public Identifier identifier { get; set; }
    public List<object> learningWorldContent { get; set; }
    public List<object> topics { get; set; }
    public List<LearningSpace> learningSpaces { get; set; }
    public List<LearningElement> learningElements { get; set; }
}

public class DslFile
{
    public LearningWorld learningWorld { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class Deployedfile
{
    public string filename { get; set; }
    public string filepath { get; set; }
    public int filesize { get; set; }
    public string fileurl { get; set; }
    public int timemodified { get; set; }
    public string mimetype { get; set; }
}

public class H5Pactivity
{
    public int id { get; set; }
    public int course { get; set; }
    public string name { get; set; }
    public int timecreated { get; set; }
    public int timemodified { get; set; }
    public string intro { get; set; }
    public int introformat { get; set; }
    public int grade { get; set; }
    public int displayoptions { get; set; }
    public int enabletracking { get; set; }
    public int grademethod { get; set; }
    public int coursemodule { get; set; }
    public int context { get; set; }
    public List<object> introfiles { get; set; }
    public List<Package> package { get; set; }
    public Deployedfile deployedfile { get; set; }
}

public class Package
{
    public string filename { get; set; }
    public string filepath { get; set; }
    public int filesize { get; set; }
    public string fileurl { get; set; }
    public int timemodified { get; set; }
    public string mimetype { get; set; }
    public bool isexternalfile { get; set; }
}

public class H5PResponseDto
{
    public List<H5Pactivity> h5Pactivities { get; set; }
    public List<object> warnings { get; set; }
}

