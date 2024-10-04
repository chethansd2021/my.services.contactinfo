using Asp.Versioning;
using FluentValidation.AspNetCore;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using my.services.contactinfo.Application;
using my.services.contactinfo.Application.Services;
using my.services.contactinfo.Domain.Constants;
using my.services.contactinfo.Domain.Settings;
using my.services.contactinfo.Infrastructure;
using my.services.contactinfo.Infrastructure.Respositories;
using my.services.contactinfo.Web.HealthChecks;
using my.services.contactinfo.Web.Logging;
using my.services.contactinfo.Web.Middleware;
using my.services.contactinfo.Web.Validation;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;
[assembly: InternalsVisibleTo(assemblyName: "my.services.contactinfo-Tests")]

namespace my.services.contactinfo.Web
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IWebHostEnvironment Env { get; set; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var serviceSettings = new GeneralServiceSettings();
            Configuration.GetSection("ServiceSettings").Bind(serviceSettings);
            services.AddSingleton(Options.Create(serviceSettings));

            // Logging
            var loggingSettings = new LogSettings();
            Configuration.GetSection("LogSettings").Bind(loggingSettings);
            services.AddApplicationLogging(Env, loggingSettings, serviceSettings);

            // Stitch
            services.AddApplicationServices();

            services.AddInfraServices(new InfraSettings
            {
                HealthCheckTags = new string[] { Constants.HealthCheckTags.Readiness },
            });

            // 1
            services.AddOptions();
            services.Configure<Domain.Settings.SwaggerSettings>(Configuration.GetSection(nameof(Domain.Settings.SwaggerSettings)));


            // 1
            services.AddApiVersioning(cfg =>
            {
                cfg.DefaultApiVersion = new ApiVersion(1, 0);
                cfg.AssumeDefaultVersionWhenUnspecified = true;
                cfg.ReportApiVersions = true;
                cfg.ApiVersionReader = new HeaderApiVersionReader("X-Version");
            });

            // 4
            services.AddHttpContextAccessor();
            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer();

            // 5
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                                   .AllowAnyMethod()
                                                                    .AllowAnyHeader()));
            // 8
            services.AddControllers();
            // Infrastructure
            services.AddSingleton<IContactRepository, ContactRepository>();
            services.AddSingleton<ContactService>();

            // Application
            services.AddTransient<ContactService>();

            // Domain
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<ContactValidator>();
            services.AddSingleton<IConfiguration>(Configuration);
            // 9
            services.AddSwaggerGen(c =>
            {
                //c.ConfigureSwaggerForOauth();
                //c.OperationFilter<CorrellationIdOperationFilter>();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = $"ContactsInfo - {Env.EnvironmentName} - {serviceSettings.Version}", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            // Add memory cache for general caching operations
            services.AddMemoryCache();

            services.AddHealthChecks();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration configuration, IHostApplicationLifetime lifetime)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                var swaggerSettings = new Domain.Settings.SwaggerSettings();
                configuration.GetSection(nameof(Domain.Settings.SwaggerSettings)).Bind(swaggerSettings);
                app.UseSwagger(opt =>
                {
                    opt.RouteTemplate = swaggerSettings.JsonRoute;
                });
                app.UseSwaggerUI(opt =>
                {
                    opt.SwaggerEndpoint(swaggerSettings.UiEndpoint, swaggerSettings.Description);
                });
            }

            if (!env.IsDevelopment())
            {
                lifetime.ApplicationStopping.Register(() =>
                {
                    // Give Kubernetes a chance to unregister us from the load balancer before we stop
                    // See https://blog.markvincze.com/graceful-termination-in-kubernetes-with-asp-net-core/
                    Log.Information("Application is shutting down after a 20-second delay");
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(20.0));
                });
            }

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = WriteHealthCheckResponse
            });

            app.UseHealthChecks("/health/live", new HealthCheckOptions
            {
                ResponseWriter = WriteHealthCheckResponse,
                Predicate = (check) => !check.Tags.Contains(Constants.HealthCheckTags.Readiness),
            });
            app.UseHealthChecks("/health/ready", new HealthCheckOptions
            {
                ResponseWriter = WriteHealthCheckResponse,
                Predicate = (check) => check.Tags.Contains(Constants.HealthCheckTags.Readiness),
            });

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseHttpsRedirection();

            app.UseCors("AllowAll");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private Task WriteHealthCheckResponse(HttpContext httpContext, HealthReport result)
        {
            httpContext.Response.ContentType = "application/json";
            var response = new HealthCheckResponseDto
            {
                Status = result.Status.ToString(),
                HealthChecks = result.Entries.Select(x => new HealthCheckDto
                {
                    Component = x.Key,
                    Status = x.Value.Status.ToString(),
                    Description = x.Value.Description
                }),
                Duration = result.TotalDuration
            };

            return httpContext.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
