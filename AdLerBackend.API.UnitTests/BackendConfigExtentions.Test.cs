using AdLerBackend.API.Properties;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdLerBackend.API.UnitTests;

public class BackendConfigExtentionsTest
{
    [Test]
    public void AddAndValidateBackendConfig_WhenCalledWithValidConfig_ShouldNotThrowException()
    {
        // Arrange
        var services = new ServiceCollection();
        var configData = new Dictionary<string, string>
        {
            {"ASPNETCORE_ADLER_MOODLEURL", "https://moodle.example.com"},
            {"ASPNETCORE_ENVIRONMENT", "Development"}
        };
        var configuration = new ConfigurationBuilder().AddInMemoryCollection(configData).Build();

        // Act
        TestDelegate act = () => services.AddAndValidateBackendConfig(configuration);

        // Assert that it does not throw an exception
        Assert.DoesNotThrow(act);

        // Assert that the configuration is added to the service collection and is accessible
        var serviceProvider = services.BuildServiceProvider();
        var myConfig = serviceProvider.GetRequiredService<BackendConfig>();
        Assert.That(myConfig.ASPNETCORE_ADLER_MOODLEURL, Is.EqualTo("https://moodle.example.com"));
    }
}