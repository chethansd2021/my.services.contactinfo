namespace my.services.contactinfo.Web.HealthChecks
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;

    public class HealthCheckResponseDto
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheckDto> HealthChecks { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
