namespace AdLerBackend.Application.Common.Interfaces;

public interface ISerialization
{
    public Task<TClass> GetObjectFromJsonStreamAsync<TClass>(Stream stream);

    public TClass GetObjectFromJsonString<TClass>(string jsonString);

    public string GetJsonStringFromObject<TClass>(TClass objectToSerialize);

    public bool IsValidJsonString(string jsonString);
}