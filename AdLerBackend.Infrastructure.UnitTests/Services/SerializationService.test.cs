using AdLerBackend.Infrastructure.Services;

namespace AdLerBackend.Infrastructure.UnitTests.Services;

public class SerializationServiceTest
{
    [Test]
    public async Task Deserialize_Valid_CanSerializeFromString()
    {
        // Arrange
        var service = new SerializationService();

        // Act
        var result = service.GetObjectFromJsonString<Root>(
            "{\"browsers\":{\"firefox\":{\"name\":\"Firefox\",\"pref_url\":\"about:config\",\"releases\":{\"1\":{\"release_date\":\"2004-11-09\",\"status\":\"retired\",\"engine\":\"Gecko\",\"engine_version\":\"1.7\"}}}}}");

        // Assert
        Assert.NotNull(result);
    }

    [TestCase("{}", true)]
    [TestCase("string", false)]
    public async Task IsValidJsonString(string json, bool expected)
    {
        // Arrange
        var service = new SerializationService();

        // Act
        var result = service.IsValidJsonString(json);

        // Assert
        Assert.AreEqual(expected, result);
    }
    
    [Test]
    public async Task Serialize_Valid_CanSerializeToString()
    {
        // Arrange
        var service = new SerializationService();

        // Act
        var result = service.ClassToJsonString(new BrokenClass {foo = 1});

        // Assert
        Assert.AreEqual("{\"foo\":1}", result);
    }
}

public class BrokenClass
{
    public int foo { get; set; }
}

public class _1
{
    public string release_date { get; set; }
    public string status { get; set; }
    public string engine { get; set; }
    public string engine_version { get; set; }
}

public class Browsers
{
    public Firefox firefox { get; set; }
}

public class Firefox
{
    public string name { get; set; }
    public string pref_url { get; set; }
    public Releases releases { get; set; }
}

public class Releases
{
    public _1 _1 { get; set; }
}

public class Root
{
    public Browsers browsers { get; set; }
}