using AdLerBackend.Application.Common.Responses.World;

namespace AdLerBackend.Application.Common.Utils;

public static class IdExtractor
{
    public static Guid GetUuidFromQuestionId(int questionId, AdaptivityElement adaptivityElement)
    {
        foreach (var question in from task in adaptivityElement.AdaptivityContent.AdaptivityTasks
                 from question in task.AdaptivityQuestions
                 where question.QuestionId == questionId
                 select question)
            return question.QuestionUuid;

        throw new Exception("No uuid for the Adaptivity Question found!");
    }

    public static int GetQuestionIdFromUuid(Guid uuid, AdaptivityElement adaptivityElement)
    {
        foreach (var question in from task in adaptivityElement.AdaptivityContent.AdaptivityTasks
                 from question in task.AdaptivityQuestions
                 where question.QuestionUuid == uuid
                 select question)
            return question.QuestionId;

        throw new Exception("No id for the Adaptivity Question found!");
    }

    public static int GetTaskIdFromUuid(Guid uuid, AdaptivityElement adaptivityElement)
    {
        foreach (var task in from task in adaptivityElement.AdaptivityContent.AdaptivityTasks
                 where task.TaskUuid == uuid
                 select task)
            return task.TaskId;

        throw new Exception("No id for the Adaptivity Task found!");
    }
}