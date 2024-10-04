using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using my.services.contactinfo.Web;
using my.services.contactinfo.Web.Controllers;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace my.services.contactinfo_Tests
{
    public class DependencyInjectionTests
    {
        private static IServiceProvider Setup(IEnumerable<Type> additionalTypes)
        {
            // Setup dependency injection
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder
                        .SetBasePath(AppContext.BaseDirectory)
                        .AddJsonFile("appsettings.json", optional: false)
                        .AddEnvironmentVariables()
                        .Build();
                })
                .ConfigureServices((context, services) =>
                {
                    var env = new Mock<IWebHostEnvironment>();
                    env.Setup(f => f.EnvironmentName).Returns("unit-test");

                    var start = new Startup(context.Configuration, env.Object);
                    start.ConfigureServices(services);

                    // Register the additional types
                    foreach (var h in additionalTypes)
                    {
                        services.AddScoped(h);
                    }
                });
            return builder.Build().Services;
        }

        [Fact]
        public void ConfigureService_DependencyInjection_MediatorHandlers()
        {
            // Get all mediator handlers
            var handlerType = typeof(IRequestHandler<,>);
            var handlers = typeof(ServiceSettingsQueryHandler).Assembly
                .GetTypes()
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && (handlerType.IsAssignableFrom(i) || handlerType.IsAssignableFrom(i.GetGenericTypeDefinition()))))
                .ToList();

            // Setup dependency injection
            var services = Setup(handlers);

            // Verify that each handler can be constructed 
            Assert.All(handlers, h =>
            {
                var svc = services.GetRequiredService(h);
                svc.ShouldNotBeNull();
            });
        }


        [Fact]
        public void ConfigureService_DependencyInjection_Controllers()
        {
            // Get all mediator handlers
            var handlerType = typeof(ControllerBase);
            var handlers = typeof(ContactsController).Assembly
                .GetTypes()
                .Where(t => handlerType.IsAssignableFrom(t))
                .ToList();

            // Setup dependency injection
            var services = Setup(handlers);

            // Verify that each handler can be constructed 
            Assert.All(handlers, h =>
            {
                var svc = services.GetRequiredService(h);
                svc.ShouldNotBeNull();
            });
        }
    }

}
