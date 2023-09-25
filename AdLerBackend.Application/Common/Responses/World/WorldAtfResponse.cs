// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable ClassNeverInstantiated.Global

#pragma warning disable CS8618

using System.Text.Json.Serialization;
using AdLerBackend.Application.Configuration;

namespace AdLerBackend.Application.Common.Responses.World;
// This is the ATF File format

[JsonDerivedType(typeof(WorldAtfResponse), JsonTypes.AtfType)]
public class WorldAtfResponse
{
    public string FileVersion { get; set; }
    public string AmgVersion { get; set; }
    public string Author { get; set; }
    public string Language { get; set; }
    public World World { get; set; }
}

[JsonDerivedType(typeof(Space), JsonTypes.LearningSpaceType)]
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

[JsonDerivedType(typeof(Topic), JsonTypes.LearningTopicType)]
public class Topic
{
    public int TopicId { get; set; }
    public string TopicName { get; set; }
    public List<int> TopicContents { get; set; }
}

[JsonDerivedType(typeof(World), JsonTypes.LearningWorldType)]
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

[JsonDerivedType(typeof(BaseElement), JsonTypes.BaseLearningElementType)]
[JsonDerivedType(typeof(Element), JsonTypes.LearningElementType)]
[JsonDerivedType(typeof(AdaptivityElement), JsonTypes.AdaptivityLearningElementType)]
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

[JsonDerivedType(typeof(AdaptivityContent), JsonTypes.AdaptivityContentType)]
public class AdaptivityContent
{
    public string IntroText { get; set; }
    public bool ShuffleTasks { get; set; }
    public List<AdaptivityTask> AdaptivityTasks { get; set; }
}

[JsonDerivedType(typeof(AdaptivityTask), JsonTypes.AdaptivityTaskType)]
public class AdaptivityTask
{
    public int TaskId { get; set; }
    public Guid TaskUuid { get; set; }
    public string TaskTitle { get; set; }
    public bool Optional { get; set; }
    public int RequiredDifficulty { get; set; }
    public List<AdaptivityQuestion> AdaptivityQuestions { get; set; }
}

[JsonDerivedType(typeof(AdaptivityQuestion), JsonTypes.AdaptivityQuestionType)]
public class AdaptivityQuestion
{
    public string QuestionType { get; set; }
    public int QuestionId { get; set; }
    public Guid QuestionUuid { get; set; }
    public int QuestionDifficulty { get; set; }
    public string QuestionText { get; set; }
    public List<AdaptivityTrigger> AdaptivityRules { get; set; }
    public List<AdaptivityQuestionAnswer> Choices { get; set; }
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

[JsonDerivedType(typeof(CorrectnessTrigger), JsonTypes.CorrectnessTriggerType)]
public class AdaptivityTrigger
{
    public int TriggerId { get; set; }
    public string TriggerCondition { get; set; }
    public AdaptivityActionBase AdaptivityAction { get; set; }
}

public class CorrectnessTrigger : AdaptivityTrigger
{
}

[JsonDerivedType(typeof(AdaptivityQuestionAnswer), JsonTypes.AdaptivityQuestionAnswerType)]
public class AdaptivityQuestionAnswer
{
    public string AnswerText { get; set; }
}