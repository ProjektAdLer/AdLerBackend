using System.Configuration;
using AdLerBackend.Application.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace AdLerBackend.API.UnitTests;

[TestFixture]
public class BackendConfigExtensionsTests
{
    [SetUp]
    public void SetUp()
    {
        _services = new ServiceCollection();
        var configData = new Dictionary<string, string>
        {
            {"ASPNETCORE_ENVIRONMENT", "Production"},
            {"ASPNETCORE_ADLER_MOODLEURL", "https://moodle.example.com"},
            {"ASPNETCORE_ADLER_ADLERENGINEURL", "https://adler-engine.example.com"},
            {"ASPNETCORE_DBPASSWORD", "strongPassword"},
            {"ASPNETCORE_DBUSER", "dbUser"},
            {"ASPNETCORE_DBNAME", "dbName"},
            {"ASPNETCORE_DBHOST", "dbHost"},
            {"ASPNETCORE_DBPORT", "1234"},
            {"ASPNETCORE_ADLER_HTTPPORT", "80"}
        };

        var memoryConfigurationSource = new MemoryConfigurationSource {InitialData = configData};
        _configuration = new ConfigurationBuilder().Add(memoryConfigurationSource).Build();
    }

    private IServiceCollection _services;
    private IConfigurationSection _configurationSection;
    private IConfiguration _configuration;

    [Test]
    public void AddAndValidateBackendConfig_WithValidBackendConfig_ConfigurationIsValid()
    {
        // Arrange
        var validBackendConfig = new BackendConfig
        {
            Environment = "Production",
            MoodleUrl = "https://moodle.example.com",
            AdLerEngineUrl = "https://adler-engine.example.com",
            DbPassword = "strongPassword",
            DbUser = "dbUser",
            DbName = "dbName",
            DbHost = "dbHost",
            DbPort = "1234"
        };

        // Act
        _services.AddAndValidateBackendConfig(_configuration);
        var serviceProvider = _services.BuildServiceProvider();
        var resolvedBackendConfig = serviceProvider.GetRequiredService<BackendConfig>();

        // Assert
        Assert.That(resolvedBackendConfig, Is.Not.Null);
        Assert.That(resolvedBackendConfig.ToString(), Is.EqualTo(validBackendConfig.ToString()));
    }

    [Test]
    public void AddAndValidateBackendConfig_WithInvalidBackendConfig_ThrowsConfigurationErrorsException()
    {
        // Arrange
        var invalidConfigData = new Dictionary<string, string>
        {
            {"ASPNETCORE_ENVIRONMENT", "InvalidEnvironment"},
            {"ASPNETCORE_ADLER_MOODLEURL", "https://moodle.example.com"}
        };

        var memoryConfigurationSource = new MemoryConfigurationSource {InitialData = invalidConfigData};
        var invalidConfiguration = new ConfigurationBuilder().Add(memoryConfigurationSource).Build();

        // Act
        _services.AddAndValidateBackendConfig(invalidConfiguration);
        var serviceProvider = _services.BuildServiceProvider();

        // Assert
        Assert.Throws<ConfigurationErrorsException>(() => serviceProvider.GetRequiredService<BackendConfig>());
    }

}