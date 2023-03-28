namespace AdLerBackend.Infrastructure.Moodle;

public class MoodleUtils
{
    public string ConvertFileStreamToBase64(Stream stream)
    {
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        var bytes = memoryStream.ToArray();
        return Convert.ToBase64String(bytes);
    }
}