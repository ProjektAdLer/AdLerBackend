// ReSharper disable UnusedAutoPropertyAccessor.Global

// ReSharper disable PropertyCanBeMadeInitOnly.Global

// ReSharper disable ClassNeverInstantiated.Global

using System.Text.Json.Serialization;

#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.World;

// This is Version 0.5 of the ATF File

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
    public List<BaseElement> Elements { get; set; }
    public string? EvaluationLink { get; set; }
}

public class Element : BaseElement
{
    public int ElementMaxScore { get; set; }
    public string ElementModel { get; set; }
}

public class AdaptivityElement : Element
{
    public AdaptivityContent AdaptivityContent { get; set; }
}

[JsonDerivedType(typeof(BaseElement), "BaseLearningElement")]
[JsonDerivedType(typeof(Element), "LearningElement")]
[JsonDerivedType(typeof(AdaptivityElement), "AdaptivityLearningElement")]
public class BaseElement
{
    public int ElementId { get; set; }
    public string ElementUuid { get; set; }
    public string ElementName { get; set; }
    public string ElementDescription { get; set; }
    public List<string> ElementGoals { get; set; }
    public string ElementCategory { get; set; }
    public string ElementFileType { get; set; }
}

public class AdaptivityContent
{
    public string IntroText { get; set; }
    public bool ShuffleTasks { get; set; }
    public List<AdaptivityTask> AdaptivityTasks { get; set; }
}

public class AdaptivityTask
{
    public int TaskId { get; set; }
    public Guid TaskUuid { get; set; }
    public string TaskTitle { get; set; }
    public bool Optional { get; set; }
    public int RequiredDifficulty { get; set; }
    public List<AdaptivityQuestion> AdaptivityQuestions { get; set; }
}

public class AdaptivityQuestion
{
    public string QuestionType { get; set; }
    public int QuestionId { get; set; }
    public Guid QuestionUuid { get; set; }
    public int QuestionDifficulty { get; set; }
    public string QuestionText { get; set; }
    public List<AdaptivityRule> AdaptivityRules { get; set; }
}

//[JsonDerivedType(typeof(AdaptivityActionBase), "base")]
[JsonDerivedType(typeof(CommentAction), "AdaptivityCommentAction")]
[JsonDerivedType(typeof(ElementReferenceAction), "AdaptivityElementReferenceAction")]
[JsonDerivedType(typeof(ContentReferenceAction), "AdaptivityContentReferenceAction")]
public abstract class AdaptivityActionBase
{
    public string AdaptivityActionType { get; set; }
}

public class CommentAction : AdaptivityActionBase
{
    public string CommentText { get; set; }
}

public class ElementReferenceAction : AdaptivityActionBase
{
    public string CommentText { get; set; }
    public int ElementId { get; set; } // ID of the Learning Element to be referenced
}

public class ContentReferenceAction : AdaptivityActionBase
{
    public string CommentText { get; set; }
    public int ElementId { get; set; } // ID of the Content to be referenced (which is also a learning element)
}

public class AdaptivityRule
{
    public int TriggerId { get; set; }
    public string TriggerType { get; set; }
    public string TriggerCondition { get; set; }
    public List<AdaptivityActionBase> AdaptivityActions { get; set; }
}