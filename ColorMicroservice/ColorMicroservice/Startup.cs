using ColorMicroservice.API.Middleware.Exceptions;
using ColorMicroservice.Infrastructure;
using ColorMicroservice.Shared.Initialization;
using NLog;
using NLog.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace ColorMicroservice.API;

internal sealed class Startup
{
    private const string ServiceName = "ColorMicroservice";
    private readonly ILogger _logger;

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        _logger = LogManager.Setup()
            .LoadConfigurationFromSection(configuration)
            .GetCurrentClassLogger();

        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _logger.Info("Application registration started");

        var assemblies = AssemblyLoader.LoadAssemblies(ServiceName).ToArray();

        services.AddControllers();
        services.AddControllerRequestHandler();
        services.AddCors();
        services.AddSwagger(ServiceName, "v1");
        services.AddMediatorFromAssemblies(assemblies);
        services.AddScoped<ExceptionHandlerMiddleware>();

        services.AddFeatures(Configuration, assemblies);
        services.AddInfrastructure(Configuration, assemblies);

        _logger.Trace("Core features, infrastructure and services registered");

        services.AddHealthChecks();
        _logger.Trace("Health checks registerd");

        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        _logger.Info("Application registration completed");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseSwagger();
            app.UseDeveloperExceptionPage();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", ServiceName);
            });
        }

        app.UseCors(c => c
            .AllowAnyMethod()
            .AllowAnyHeader()
            .SetIsOriginAllowed(origin => true)); //Configure CORS here

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionHandlerMiddleware>();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHealthChecks("/health");
            endpoints.MapControllers().AllowAnonymous(); //Don't forget to remove AllowAnonymous when auth is used
        });
    }
}
