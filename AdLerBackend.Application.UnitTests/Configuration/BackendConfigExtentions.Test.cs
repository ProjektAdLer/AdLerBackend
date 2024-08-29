using AdLerBackend.API;
using AdLerBackend.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdLerBackend.Application.UnitTests.Configuration;

public class BackendConfigExtentionsTest
{
    [Test]
    public void AddAndValidateBackendConfig_WhenCalledWithValidConfig_ShouldNotThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string>
        {
            { "ASPNETCORE_ADLER_MOODLEURL", "https://moodle.example.com" },
            { "ASPNETCORE_ENVIRONMENT", "Development" },
            { "ASPNETCORE_DBPASSWORD", "test_password" },
            { "ASPNETCORE_DBUSER", "test_user" },
            { "ASPNETCORE_DBNAME", "test_db_name" },
            { "ASPNETCORE_DBHOST", "localhost" },
            { "ASPNETCORE_DBPORT", "5432" },
            { "ASPNETCORE_ADLER_ADLERENGINEURL", "https://adlerengine.example.com" }
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();

        // Act
        TestDelegate act = () => services.AddAndValidateBackendConfig(configuration);

        // Assert that it does not throw an exception
        Assert.DoesNotThrow(act);

        // Assert that the configuration is added to the service collection and is accessible
        var serviceProvider = services.BuildServiceProvider();
        var myConfig = serviceProvider.GetRequiredService<BackendConfig>();
        Assert.Multiple(() =>
        {
            Assert.That(myConfig.MoodleUrl, Is.EqualTo("https://moodle.example.com"));
            Assert.That(myConfig.DbPassword, Is.EqualTo("test_password"));
            Assert.That(myConfig.DbUser, Is.EqualTo("test_user"));
            Assert.That(myConfig.DbName, Is.EqualTo("test_db_name"));
            Assert.That(myConfig.DbHost, Is.EqualTo("localhost"));
            Assert.That(myConfig.DbPort, Is.EqualTo("5432"));
            Assert.That(myConfig.AdLerEngineUrl, Is.EqualTo("https://adlerengine.example.com"));
        });
    }
}