#pragma warning disable CS8618
using System.Text.Json.Serialization;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace AdLerBackend.Application.Common.DTOs;

public class Actor
{
    public string Name { get; set; }
    public string Mbox { get; set; }
    public string ObjectType { get; set; }
}

public class Category
{
    public string Id { get; set; }
    public string ObjectType { get; set; }
}

public class Context
{
    public ContextActivities ContextActivities { get; set; }
}

public class ContextActivities
{
    public List<Category> Category { get; set; }
}

public abstract class Definition
{
    public Extensions Extensions { get; set; }
    public Name Name { get; set; }
}

public class Display
{
    [JsonPropertyName("en-US")] public string EnUs { get; set; }
}

public class Extensions
{
    [JsonPropertyName("http://h5p.org/x-api/h5p-local-content-id")]
    public string HttpH5POrgXApiH5PLocalContentId { get; set; }
}

public class Name
{
    [JsonPropertyName("en-US")] public string EnUs { get; set; }
}

public class Object
{
    public string Id { get; set; }
    public string ObjectType { get; set; }
    public Definition Definition { get; set; }
}

public class Result
{
    public Score Score { get; set; }
    public bool Completion { get; set; }
    public string Duration { get; set; }
    public bool Success { get; set; }
}

public class RawH5PEvent
{
    public Actor Actor { get; set; }
    public Verb Verb { get; set; }
    public Object Object { get; set; }
    public Context Context { get; set; }
    public Result Result { get; set; }
}

public class Score
{
    public int Min { get; set; }
    public int Max { get; set; }
    public int Raw { get; set; }
    public double Scaled { get; set; }
}

public class Verb
{
    public string Id { get; set; }
    public Display Display { get; set; }
}