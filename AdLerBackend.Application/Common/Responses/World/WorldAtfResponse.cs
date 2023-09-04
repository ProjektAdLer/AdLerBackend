﻿// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable PropertyCanBeMadeInitOnly.Global

// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.World;

// This is Version 0.5 of the ATF File
public class Element
{
    public int ElementId { get; set; }
    public string ElementUuid { get; set; }
    public string ElementName { get; set; }
    public string ElementDescription { get; set; }
    public List<string> ElementGoals { get; set; }
    public string ElementCategory { get; set; }
    public string ElementFileType { get; set; }
    public int ElementMaxScore { get; set; }
    public string ElementModel { get; set; }
}

public class WorldAtfResponse
{
    public string FileVersion { get; set; }
    public string AmgVersion { get; set; }
    public string Author { get; set; }
    public string Language { get; set; }
    public World World { get; set; }
}

public class Space
{
    public int SpaceId { get; set; }
    public string SpaceUuid { get; set; }
    public string SpaceName { get; set; }
    public string SpaceDescription { get; set; }
    public List<string> SpaceGoals { get; set; }
    public List<int?> SpaceSlotContents { get; set; }
    public int RequiredPointsToComplete { get; set; }
    public string RequiredSpacesToEnter { get; set; }
    public string SpaceTemplate { get; set; }
    public string SpaceTemplateStyle { get; set; }
}

public class Topic
{
    public int TopicId { get; set; }
    public string TopicName { get; set; }
    public List<int> TopicContents { get; set; }
}

public class World
{
    public string WorldName { get; set; }
    public string WorldUuid { get; set; }
    public string WorldDescription { get; set; }
    public List<string> WorldGoals { get; set; }
    public List<Topic> Topics { get; set; }
    public List<Space> Spaces { get; set; }
    public List<Element> Elements { get; set; }
    public string? EvaluationLink { get; set; }
}