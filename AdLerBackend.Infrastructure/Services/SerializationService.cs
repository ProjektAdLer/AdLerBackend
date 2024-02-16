using System.Text.Json;
using AdLerBackend.Application.Common.Interfaces;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace AdLerBackend.Infrastructure.Services;

public class SerializationService : ISerialization
{
    public TClass GetObjectFromJsonString<TClass>(string jsonString)
    {
        var retVal = JsonSerializer.Deserialize<TClass>(jsonString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? throw new Exception("Could not deserialize String");
        return retVal;
    }

    public bool IsValidJsonString(string potentialJsonString)
    {
        if (string.IsNullOrWhiteSpace(potentialJsonString)) return false;

        try
        {
            var obj = JsonConvert.DeserializeObject(potentialJsonString);
            return true;
        }
        catch (JsonReaderException)
        {
            return false;
        }
        catch (Exception) // Some other exception
        {
            return false;
        }
    }

    public string ClassToJsonString(object classToSerialize)
    {
        return JsonSerializer.Serialize(classToSerialize);
    }
}