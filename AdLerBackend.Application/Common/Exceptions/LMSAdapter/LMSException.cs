namespace AdLerBackend.Application.Common.Exceptions.LMSAdapter;

public class LmsException : Exception
{
    public LmsException()
    {
    }


    public LmsException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public LmsException(string errorMessage) : base(errorMessage)
    {
    }

    public string LmsErrorCode { get; set; } = "";
}