// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable CS8618

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using AdLerBackend.Application.Configuration;

namespace AdLerBackend.Application.Common.Responses.World;
// This is the ATF File format (2.3.0)

[JsonDerivedType(typeof(WorldAtfResponse), JsonTypes.AtfType)]
public class WorldAtfResponse
{
    [Required] public string FileVersion { get; set; }
    [Required] public string AmgVersion { get; set; }
    public string? Author { get; set; }
    [Required] public string Language { get; set; }
    [Required] public World World { get; set; }
}

[JsonDerivedType(typeof(Space), JsonTypes.LearningSpaceType)]
public class Space
{
    [Required] public int SpaceId { get; set; }
    [Required] public string SpaceUuid { get; set; }
    [Required] public string SpaceName { get; set; }
    public string? SpaceDescription { get; set; }
    public List<string>? SpaceGoals { get; set; }
    [Required] public List<int?> SpaceSlotContents { get; set; }
    public int? RequiredPointsToComplete { get; set; }
    [Required] public string RequiredSpacesToEnter { get; set; }
    [Required] public string SpaceTemplate { get; set; }
    [Required] public string SpaceTemplateStyle { get; set; }
    public SpaceStory? SpaceStory { get; set; }
}

[JsonDerivedType(typeof(SpaceStory), JsonTypes.LearningSpaceStoryType)]
public class SpaceStory
{
    public StoryElement? IntroStory { get; set; }
    public StoryElement? OutroStory { get; set; }
}

[JsonDerivedType(typeof(StoryElement), JsonTypes.SimpleSpaceStoryElement)]
public class StoryElement
{
    [Required] public List<string> StoryTexts { get; set; }
    [Required] public string ElementModel { get; set; }
    public string? ModelFacialExpression { get; set; }
    public string? StoryNpcName { get; set; }
}

[JsonDerivedType(typeof(Topic), JsonTypes.LearningTopicType)]
public class Topic
{
    [Required] public int TopicId { get; set; }
    [Required] public string TopicName { get; set; }
    [Required] public List<int> TopicContents { get; set; }
}

[JsonDerivedType(typeof(FrameStory), JsonTypes.FrameStoryType)]
public class FrameStory
{
    public string? FrameStoryIntro { get; set; }
    public string? FrameStoryOutro { get; set; }
}

[JsonDerivedType(typeof(World), JsonTypes.LearningWorldType)]
public class World
{
    [Required] public string WorldName { get; set; }
    [Required] public string WorldUuid { get; set; }
    public string? WorldDescription { get; set; }
    public List<string>? WorldGoals { get; set; }
    public string? Theme { get; set; }
    public FrameStory? FrameStory { get; set; }
    public List<Topic>? Topics { get; set; }
    [Required] public List<Space> Spaces { get; set; }
    [Required] public List<BaseElement> Elements { get; set; }
    public string? EvaluationLink { get; set; }
    public string? WorldGradingStyle { get; set; }
}

public class Element : BaseElement
{
    public string? ElementDescription { get; set; }
    public List<string>? ElementGoals { get; set; }
    [Required] public int ElementMaxScore { get; set; }
    [Required] public string ElementModel { get; set; }
    public int? ElementDifficulty { get; set; }
    public string? ElementTemplate { get; set; }
    public int? ElementEstimatedTimeMinutes { get; set; }
}

public class AdaptivityElement : Element
{
    [Required] public AdaptivityContent AdaptivityContent { get; set; }
}

[JsonDerivedType(typeof(BaseElement), JsonTypes.BaseLearningElementType)]
[JsonDerivedType(typeof(Element), JsonTypes.LearningElementType)]
[JsonDerivedType(typeof(AdaptivityElement), JsonTypes.AdaptivityLearningElementType)]
public class BaseElement
{
    [Required] public int ElementId { get; set; }
    [Required] public Guid ElementUuid { get; set; }
    [Required] public string ElementName { get; set; }
    [Required] public string ElementCategory { get; set; }
    [Required] public string ElementFileType { get; set; }
}

[JsonDerivedType(typeof(AdaptivityContent), JsonTypes.AdaptivityContentType)]
public class AdaptivityContent
{
    public string? IntroText { get; set; }
    [Required] public List<AdaptivityTask> AdaptivityTasks { get; set; }
}

[JsonDerivedType(typeof(AdaptivityTask), JsonTypes.AdaptivityTaskType)]
public class AdaptivityTask
{
    [Required] public int TaskId { get; set; }
    [Required] public Guid TaskUuid { get; set; }

    [Required] public string TaskTitle { get; set; }

    // the Optional Flag ist Deprecated
    public bool? Optional { get; set; }
    public int? RequiredDifficulty { get; set; }
    [Required] public List<AdaptivityQuestion> AdaptivityQuestions { get; set; }
}

[JsonDerivedType(typeof(AdaptivityQuestion), JsonTypes.AdaptivityQuestionType)]
public class AdaptivityQuestion
{
    [Required] public string QuestionType { get; set; }
    [Required] public int QuestionId { get; set; }
    [Required] public Guid QuestionUuid { get; set; }
    [Required] public int QuestionDifficulty { get; set; }
    [Required] public string QuestionText { get; set; }
    [Required] public List<AdaptivityTrigger> AdaptivityRules { get; set; }
    [Required] public List<AdaptivityQuestionAnswer> Choices { get; set; }
}

//[JsonDerivedType(typeof(AdaptivityActionBase), "base")]
[JsonDerivedType(typeof(CommentAction), JsonTypes.AdaptivityCommentActionType)]
[JsonDerivedType(typeof(ElementReferenceAction), JsonTypes.AdaptivityElementReferenceActionType)]
[JsonDerivedType(typeof(ContentReferenceAction), JsonTypes.AdaptivityContentReferenceActionType)]
public abstract class AdaptivityActionBase
{
}

public class CommentAction : AdaptivityActionBase
{
    [Required] public string CommentText { get; set; }
}

public class ElementReferenceAction : AdaptivityActionBase
{
    [Required] public int ElementId { get; set; } // ID of the Learning Element to be referenced
    [Required] public string? HintText { get; set; }
}

public class ContentReferenceAction : AdaptivityActionBase
{
    [Required]
    public int ElementId { get; set; } // ID of the Content to be referenced (which is also a learning element)

    [Required] public string? HintText { get; set; }
}

[JsonDerivedType(typeof(CorrectnessTrigger), JsonTypes.CorrectnessTriggerType)]
public class AdaptivityTrigger
{
    [Required] public int TriggerId { get; set; }
    [Required] public string TriggerCondition { get; set; }
    [Required] public AdaptivityActionBase AdaptivityAction { get; set; }
}

public class CorrectnessTrigger : AdaptivityTrigger
{
}

[JsonDerivedType(typeof(AdaptivityQuestionAnswer), JsonTypes.AdaptivityQuestionAnswerType)]
public class AdaptivityQuestionAnswer
{
    [Required] public string AnswerText { get; set; }
}