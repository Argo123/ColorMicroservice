using ColorMicroservice.Core.Abstractions.Services;
using ColorMicroservice.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ColorMicroservice.Infrastructure
{
    public static class Registration
    {
        public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies)
        {
            services.AddServices();
        }

        private static void AddServices(this IServiceCollection services)
            => services.AddScoped<IColorService, ColorService>();
    }
}
