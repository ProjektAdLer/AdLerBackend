using System.Reflection;
using AdLerBackend.Application.UnitTests.Common.Behaviours;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace AdLerBackend.Application.UnitTests;

public class ConfigureServicesTest
{
    [Test]
    public void AddApplicationServices_ConfiguresMediatR()
    {
        // Arrange
        var services = Substitute.For<IServiceCollection>();

        // Act
        services.AddApplicationServices();

        // Assert
        services.Received().AddMediatR(Assembly.GetExecutingAssembly());
    }

    [Test]
    public void AddApplicationServices_ConfiguresValidators()
    {
        // Arrange
        var services = Substitute.For<IServiceCollection>();

        // Act
        services.AddApplicationServices();

        // Assert
        services.Received(3).AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.Received().Add(Arg.Is<ServiceDescriptor>(d =>
            d.ServiceType == typeof(IValidator<TestModel>) && d.ImplementationType == typeof(TestModelValidator) &&
            d.Lifetime == ServiceLifetime.Scoped));
    }
}