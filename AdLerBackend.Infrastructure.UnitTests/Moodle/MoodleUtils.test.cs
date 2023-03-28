using AdLerBackend.Infrastructure.Moodle;

namespace AdLerBackend.Infrastructure.UnitTests.Moodle;

public class MoodleUtils_test
{
    [Test]
    public void ConvertFileStreamToBase64_ReturnsBase64EncodedString()
    {
        // Arrange
        var utils = new MoodleUtils();
        var data = new byte[] {0x00, 0x01, 0x02};
        var stream = new MemoryStream(data);

        // Act
        var result = utils.ConvertFileStreamToBase64(stream);

        // Assert
        Assert.That(result, Is.EqualTo("AAEC"));
    }
}