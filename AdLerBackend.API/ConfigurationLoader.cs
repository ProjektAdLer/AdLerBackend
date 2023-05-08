using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

#pragma warning disable CS1591

namespace AdLerBackend.API;

[ExcludeFromCodeCoverage]
public static class ConfigurationLoader
{
    public static void CreateDefaultConfigAndCrash()
    {
        File.WriteAllText("./config/config.json", JsonConvert.SerializeObject(new
        {
            useHttps = "false",
            httpPort = 80,
            moodleUrl = "Please specify moodle url"
        }, Formatting.Indented));

        // shut down program with message in dialog
        Console.WriteLine("Please edit the config file in ./config/config.json and restart the program.");
        Environment.Exit(1);
    }
}