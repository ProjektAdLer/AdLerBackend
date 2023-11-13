using System.ComponentModel.DataAnnotations;
using System.Configuration;
using AdLerBackend.Application.Configuration;
using Microsoft.Extensions.Options;

namespace AdLerBackend.API;

public static class BackendConfigExtensions
{
    public static IServiceCollection AddAndValidateBackendConfig(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<BackendConfig>(configuration);

        services.PostConfigure<BackendConfig>(myConfig =>
        {
            var context = new ValidationContext(myConfig);
            var results = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(myConfig, context, results, true);

            if (!isValid)
                throw new ConfigurationErrorsException("Configuration validation failed: " +
                                                       string.Join(", ", results.Select(x => x.ErrorMessage)));
        });

        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<BackendConfig>>().Value);

        return services;
    }
}