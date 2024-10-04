using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(assemblyName: "my.services.contactinfo-Tests")]

namespace my.services.contactinfo.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //services.AddMediatR(f => f.RegisterServicesFromAssemblyContaining<ServiceSettingsQuery>());
            //services.AddValidatorsFromAssemblyContaining<ServiceSettingsQuery>();

            return services;
        }
    }
}
