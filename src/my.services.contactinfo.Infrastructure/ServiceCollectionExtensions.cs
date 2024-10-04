using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "my.services.contactinfo-Tests")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]// for moq

namespace my.services.contactinfo.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfraServices(this IServiceCollection services, InfraSettings settings)
        {

            return services;
        }
    }

    public class InfraSettings
    {
        public string[] HealthCheckTags { get; set; }
    }
}
