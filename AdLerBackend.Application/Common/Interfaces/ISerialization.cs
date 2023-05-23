namespace AdLerBackend.Application.Common.Interfaces;

public interface ISerialization
{
    public TClass GetObjectFromJsonString<TClass>(string jsonString);

    public bool IsValidJsonString(string jsonString);
}