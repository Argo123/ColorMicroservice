using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ColorMicroservice.Shared.Initialization.Interfaces;

public interface IRegistration
{
    IServiceCollection Register(IServiceCollection services, IConfiguration configuration, params Assembly[] assemblies);
}
