using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace my.services.contactinfo.Domain.Settings
{
    /// <summary>
    /// Settings use to configure logging
    /// </summary>
    public class LogSettings
    {
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the console.
        /// </summary>
        /// <value>The console.</value>
        public ConsoleLogSettings Console { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>The file.</value>
        public FileLogSettings File { get; set; }

        /// <summary>
        /// Gets or sets the minimum level overrides.
        /// </summary>
        /// <value>The minimum level overrides.</value>
        public IDictionary<string, string> MinimumLevelOverrides { get; set; }

        /// <summary>
        /// Gets or sets the exclude paths.
        /// </summary>
        /// <value>The exclude paths.</value>
        public IEnumerable<string> ExcludePaths { get; set; }

        /// <summary>
        /// Gets or sets the exclude properties.
        /// </summary>
        /// <value>The exclude properties.</value>
        public IEnumerable<string> ExcludeProperties { get; set; }

        /// <summary>
        /// Gets or sets the tags.
        /// </summary>
        /// <value>The tags.</value>
        public IDictionary<string, object> Tags { get; set; }

        /// <summary>
        /// Request Payload Level
        /// </summary>
        public string RequestPayloadLevel { get; set; }

    }

    /// <summary>
    /// Class ConsoleLoggingSettings.
    /// </summary>
    public class ConsoleLogSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="ConsoleLogSettings" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }

        /// <summary>
        /// When enable, writes to console using default logger format. Useful for local development
        /// </summary>
        public bool UseSimpleFormat { get; set; }
    }

    /// <summary>
    /// Class FileLoggingSettings.
    /// </summary>
    public class FileLogSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FileLogSettings" /> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled { get; set; }
        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path { get; set; }
        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>The interval.</value>
        public string Interval { get; set; }
    }
}
