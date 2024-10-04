using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using my.services.contactinfo.Domain.Settings;
using Serilog;
using Serilog.Core;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Extensions.Logging;
using Serilog.Filters;
using Serilog.Formatting.Compact;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace my.services.contactinfo.Web.Logging
{
    /// <summary>
    /// Class Extensions.
    /// </summary> 
    [ExcludeFromCodeCoverage]
    public static class LoggingServiceCollectionExtenstion
    {
        /// <summary>
        /// The logging level switch
        /// </summary>
        internal static readonly LoggingLevelSwitch LoggingLevelSwitch = new LoggingLevelSwitch();

        public static void AddApplicationLogging(this IServiceCollection services, IWebHostEnvironment env, LogSettings logSettings, GeneralServiceSettings serviceSettings)
        {
            var loggerConfig = new LoggerConfiguration();
            MapOptions(loggerConfig, logSettings, serviceSettings, env.EnvironmentName);

            Log.Logger = loggerConfig.CreateLogger();
            services.AddScoped<Serilog.ILogger>(s => Log.Logger);

            // Setup Microsoft.Logging.Ilogger to use SeriLog 
            services.AddSingleton<ILoggerFactory>(services => new SerilogLoggerFactory(Log.Logger, false));
        }

        /// <summary>
        /// Maps the options.
        /// </summary>
        /// <param name="loggerOptions">The logger options.</param>
        /// <param name="appOptions">The application options.</param>
        /// <param name="loggerConfiguration">The logger configuration.</param>
        /// <param name="environmentName">Name of the environment.</param>
        private static void MapOptions(LoggerConfiguration loggerConfiguration, LogSettings loggerOptions, GeneralServiceSettings appOptions, string environmentName)
        {
            LoggingLevelSwitch.MinimumLevel = GetLogEventLevel(loggerOptions.Level);

            loggerConfiguration.Enrich.FromLogContext()
                .MinimumLevel.ControlledBy(LoggingLevelSwitch)
                .Enrich.WithProperty("Environment", environmentName)
                .Enrich.WithProperty("ApplicationName", appOptions.Name)
                .Enrich.WithProperty("Instance", appOptions.Instance)
                .Enrich.WithProperty("Version", appOptions.Version)
                .Enrich.With(new CorrelationIdHeaderEnricher("X-Correlation-ID"))
                .Enrich.FromLogContext();

            foreach (var (key, value) in loggerOptions.Tags ?? new Dictionary<string, object>())
            {
                loggerConfiguration.Enrich.WithProperty(key, value);
            }

            foreach (var (key, value) in loggerOptions.MinimumLevelOverrides ?? new Dictionary<string, string>())
            {
                var logLevel = GetLogEventLevel(value);
                loggerConfiguration.MinimumLevel.Override(key, logLevel);
            }

            loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.StartsWith("/health")));
            loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.StartsWith("/swagger")));

            loggerOptions.ExcludePaths?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty<string>("RequestPath", n => n.EndsWith(p, StringComparison.OrdinalIgnoreCase))));

            loggerOptions.ExcludeProperties?.ToList().ForEach(p => loggerConfiguration.Filter
                .ByExcluding(Matching.WithProperty(p)));

            // https://docs.datadoghq.com/logs/log_collection/csharp/?tab=serilog
            var consoleOptions = loggerOptions.Console ?? new ConsoleLogSettings();
            if (consoleOptions.Enabled)
            {
                // Simple format is useful for local debugging, where you want to review the logs in your console
                if (consoleOptions.UseSimpleFormat)
                {
                    loggerConfiguration.WriteTo.Console();
                }
                else
                {
                    loggerConfiguration.WriteTo.Console(new RenderedCompactJsonFormatter());
                }
            }

            var fileOptions = loggerOptions.File ?? new FileLogSettings();
            if (fileOptions.Enabled)
            {
                var path = string.IsNullOrWhiteSpace(fileOptions.Path) ? "logs/logs.txt" : fileOptions.Path;
                if (!Enum.TryParse<RollingInterval>(fileOptions.Interval, true, out var interval))
                {
                    interval = RollingInterval.Day;
                }

                loggerConfiguration.WriteTo.File(new RenderedCompactJsonFormatter(), path, rollingInterval: interval);
            }
        }

        /// <summary>
        /// Gets the log event level.
        /// </summary>
        /// <param name="level">The level.</param>
        /// <returns>LogEventLevel.</returns>
        internal static LogEventLevel GetLogEventLevel(string level)
            => Enum.TryParse<LogEventLevel>(level, true, out var logLevel)
                ? logLevel
                : LogEventLevel.Debug;
    }
}
