using System.Text.Json;
using AdLerBackend.Application.Common.Interfaces;

namespace AdLerBackend.Infrastructure.Services
{
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
                JsonDocument.Parse(potentialJsonString);
                return true;
            }
            catch (JsonException)
            {
                return false;
            }
        }

        public string ClassToJsonString(object classToSerialize)
        {
            return JsonSerializer.Serialize(classToSerialize);
        }
    }
}