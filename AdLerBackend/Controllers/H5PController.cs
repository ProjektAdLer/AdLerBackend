using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace AdLerBackend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class H5PController : ControllerBase
    {
        private static readonly HttpClient Client = new HttpClient();
        [HttpPost]
        public async Task<string> SendH5P(H5PInputDto data)
        {
            const string jsonString = "{\"actor\":{\"name\":\"name\",\"mbox\":\"mailto:foofoo@barbar.com\",\"objectType\":\"Agent\"},\"verb\":{\"id\":\"http://adlnet.gov/expapi/verbs/answered\",\"display\":{\"en-US\":\"answered\"}},\"object\":{\"id\":\"https://moodle.cluuub.xyz/xapi/activity/278\",\"objectType\":\"Activity\",\"definition\":{\"extensions\":{\"http://h5p.org/x-api/h5p-local-content-id\":\"vctsxms49\"},\"name\":{\"en-US\":\"MetrikenTeil1\"}}},\"context\":{\"contextActivities\":{\"category\":[{\"id\":\"http://h5p.org/libraries/H5P.InteractiveVideo-1.22\",\"objectType\":\"Activity\"}]}},\"result\":{\"score\":{\"min\":0,\"max\":13,\"raw\":1,\"scaled\":0.0769},\"completion\":true,\"duration\":\"PT175.63S\"}}";
            var json = JsonConvert.DeserializeObject<Root>(jsonString);
            if (json == null) throw new Exception("Couldn't convert json to Root object");
            json.actor.name = data.username;
            json.actor.mbox = data.email;

            json.@object.id = data.objectId;
            json.@object.definition.name.enUs = data.objectName;

            json.result.score.max = data.maxScore;
            json.result.score.raw = data.rawScore;
            json.result.score.scaled = data.scaledScore;
            json.result.completion = data.completion;
            json.result.duration = data.duration;

            var retVal = "[" + JsonConvert.SerializeObject(json, Formatting.None) + "]";
            var value = new Dictionary<string, string>
             {
                    { "component", "mod_h5pactivity" },
                    { "requestjson", retVal }
            };

            var content = new FormUrlEncodedContent(value);

            var test = await Client.PostAsync($"https://moodle.cluuub.xyz/webservice/rest/server.php?wstoken={data.wstoken}&wsfunction=core_xapi_statement_post&moodlewsrestformat=json",
                            content);


            return JsonConvert.SerializeObject(test.Content.ReadAsStringAsync().Result);
        }
    }


}

public class Response
{
    public List<bool> myArray { get; set; }
}



public class H5PInputDto
{
    [Required]
    public string wstoken { get; set; }
    [Required]
    public string username { get; set; }
    [Required]
    public string email { get; set; }
    [Required]
    public string objectId { get; set; }
    [Required]
    public string objectName { get; set; }
    [Required]
    public int maxScore { get; set; }
    [Required]
    public int rawScore { get; set; }
    [Required]
    public float scaledScore { get; set; }
    [Required]
    public Boolean completion { get; set; }
    [Required]
    public string duration { get; set; }
}



// Root json = JsonConvert.DeserializeObject<Root>(myJsonResponse);

public class Root
{
    public Actor actor { get; set; }
    public Verb verb { get; set; }
    public Object @object { get; set; }
    public Context context { get; set; }
    public Result result { get; set; }
}
public class Actor
{
    public string name { get; set; }
    public string mbox { get; set; }
    public string objectType { get; set; }
}

public class Category
{
    public string id { get; set; }
    public string objectType { get; set; }
}

public class Context
{
    public ContextActivities contextActivities { get; set; }
}

public class ContextActivities
{
    public List<Category> category { get; set; }
}

public class Definition
{
    public Extensions extensions { get; set; }
    public Name name { get; set; }
}

public class Display
{
    [JsonProperty("en-US")]
    public string enUs { get; set; }
}

public class Extensions
{
    [JsonProperty("http://h5p.org/x-api/h5p-local-content-id")]
    public string httpH5POrgXApiH5PLocalContentId { get; set; }
}

public class Name
{
    [JsonProperty("en-US")]
    public string enUs { get; set; }
}

public class Object
{
    public string id { get; set; }
    public string objectType { get; set; }
    public Definition definition { get; set; }
}

public class Result
{
    public Score score { get; set; }
    public bool completion { get; set; }
    public string duration { get; set; }
}

public class Score
{
    public int min { get; set; }
    public int max { get; set; }
    public int raw { get; set; }
    public double scaled { get; set; }
}

public class Verb
{
    public string id { get; set; }
    public Display display { get; set; }
}

