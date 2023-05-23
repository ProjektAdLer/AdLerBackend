using System.Text.Json;
using AdLerBackend.Application.Common.Exceptions.LMSBackupProcessor;
using AdLerBackend.Application.Common.Interfaces;

namespace AdLerBackend.Infrastructure.Services;

public class SerializationService : ISerialization
{
    public Task<TClass> GetObjectFromJsonStreamAsync<TClass>(Stream stream)
    {
        stream.Position = 0;

        var retVal = JsonSerializer.Deserialize<TClass>(stream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new LmsBackupProcessorException("Could not deserialize DSL file");
        return Task.FromResult(retVal);
    }

    public TClass GetObjectFromJsonString<TClass>(string jsonString)
    {
        var retVal = JsonSerializer.Deserialize<TClass>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Could not deserialize String");
        return retVal;
    }

    public string GetJsonStringFromObject<TClass>(TClass objectToSerialize)
    {
        return JsonSerializer.Serialize(objectToSerialize);
    }

    public bool IsValidJsonString(string jsonString)
    {
        return jsonString.StartsWith("{") && jsonString.EndsWith("}");
    }
}