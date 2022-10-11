using System.Text.Json.Serialization;

namespace AdLerBackend.Application.Common.Responses.Course;

public class LearningWorldDtoResponse
{
    [JsonPropertyName("LearningWorld")] public LearningWorld LearningWorld { get; set; }
}

public class Identifier
{
    public string Type { get; set; }
    public string Value { get; set; }
}

public class LearningElement
{
    public int Id { get; set; }
    public Identifier Identifier { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public string ElementCategory { get; set; }

    public List<Identifier> LearningElementValueList { get; set; }
    public int LearningSpaceParentId { get; set; }
}

public class LearningSpace
{
    public int SpaceId { get; set; }
    public Identifier Identifier { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public List<int> LearningSpaceContent { get; set; }

    public int RequiredPoints { get; set; }
    //public int IncludedPoints { get; set; }

    public List<int> Requirements { get; set; }
}

public class LearningWorld
{
    public string IdNumber { get; set; }
    public Identifier Identifier { get; set; }
    public string Description { get; set; }
    public string Goals { get; set; }
    public List<int> LearningWorldContent { get; set; }
    public List<LearningSpace> LearningSpaces { get; set; }
    public List<LearningElement> LearningElements { get; set; }
}