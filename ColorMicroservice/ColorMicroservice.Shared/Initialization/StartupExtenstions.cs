using ColorMicroservice.Shared.Controllers;
using ColorMicroservice.Shared.Controllers.Interfaces;
using ColorMicroservice.Shared.Initialization.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using NLog;
using System.Reflection;

namespace ColorMicroservice.Shared.Initialization;

public static class StartupExtensions
{
    private static readonly NLog.ILogger Logger = LogManager.GetCurrentClassLogger();

    public static IServiceCollection AddControllerRequestHandler(this IServiceCollection services)
        => services.AddScoped<IControllerRequestHandler, MediatrControllerRequestHandler>();

    public static IServiceCollection AddSwagger(
        this IServiceCollection services,
        string serviceName,
        string version)
    {
        services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.SwaggerDoc(version,
                new OpenApiInfo
                {
                    Title = serviceName,
                    Version = version
                });
        });

        Logger.Trace("> Swagger registered");

        return services;
    }

    public static IServiceCollection AddMediatorFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
        => services.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assemblies));

    public static IServiceCollection AddFeatures(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        foreach (var featureRegistration in GetRegistrationsFromAssemblies(assemblies))
        {
            services = featureRegistration.Register(services, configuration, assemblies);
        }

        return services;
    }

    private static IRegistration[] GetRegistrationsFromAssemblies(IEnumerable<Assembly> assemblies)
        => assemblies
            .SelectMany(a => a.GetTypes())
            .Where(type => type.IsAbstract is false && type.GetInterface(nameof(IRegistration)) is not null)
            .Select(type => Activator.CreateInstance(type)!)
            .Cast<IRegistration>()
            .ToArray();
}