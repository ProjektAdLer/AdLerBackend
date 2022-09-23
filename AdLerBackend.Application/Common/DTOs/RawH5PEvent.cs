using System.Text.Json.Serialization;

namespace AdLerBackend.Application.Common.DTOs;

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
    [JsonPropertyName("en-US")] public string EnUS { get; set; }
}

public class Extensions
{
    [JsonPropertyName("http://h5p.org/x-api/h5p-local-content-id")]
    public string HttpH5pOrgXApiH5pLocalContentId { get; set; }
}

public class Name
{
    [JsonPropertyName("en-US")] public string EnUS { get; set; }
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
    public bool success { get; set; }
}

public class RawH5PEvent
{
    public Actor actor { get; set; }
    public Verb verb { get; set; }
    public Object @object { get; set; }
    public Context context { get; set; }
    public Result result { get; set; }
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