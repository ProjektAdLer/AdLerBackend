using JetBrains.Annotations;

namespace AdLerBackend.Application.Configuration;

/// <summary>
///     Json types used for deserialization and serialization
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)] // To suppress Rider warnings
public static class JsonTypes
{
    public const string AtfType = "ATF";

    public const string LearningWorldType = "LearningWorld";
    public const string LearningTopicType = "LearningTopic";
    public const string LearningSpaceType = "LearningSpace";

    public const string LearningElementType = "LearningElement";
    public const string BaseLearningElementType = "BaseLearningElement";
    public const string AdaptivityLearningElementType = "AdaptivityElement";

    public const string AdaptivityContentType = "adaptivityContent";
    public const string AdaptivityTaskType = "adaptivityTask";
    public const string AdaptivityQuestionType = "adaptivityQuestion";

    public const string CorrectnessTriggerType = "correctnessTrigger";

    public const string AdaptivityQuestionAnswerType = "AdaptivityQuestionAnswer";

    public const string AdaptivityCommentActionType = "CommentAction";
    public const string AdaptivityElementReferenceActionType = "AdaptivityReferenceAction";
    public const string AdaptivityContentReferenceActionType = "AdaptivityContentReferenceAction";
    public const string LearningSpaceStoryType = "SimpleSpaceStory";
    public const string SimpleSpaceStoryElement = "SimpleSpaceStoryElement";
    public const string FrameStoryType = "FrameStory";
}