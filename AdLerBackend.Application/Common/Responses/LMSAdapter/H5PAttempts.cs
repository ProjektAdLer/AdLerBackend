#pragma warning disable CS8618
namespace AdLerBackend.Application.Common.Responses.LMSAdapter;

public class H5PAttempts
{
    public List<Usersattempt> usersattempts { get; set; }
}

public class Attempt
{
    public int success { get; set; }
}

public class Scored
{
    public List<Attempt> attempts { get; set; }
}

public class Usersattempt
{
    public Scored scored { get; set; }
}