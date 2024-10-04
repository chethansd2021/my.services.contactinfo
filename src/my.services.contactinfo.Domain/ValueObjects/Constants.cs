namespace my.services.contactinfo.Domain.Constants
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class Constants
    {
        public const int NoFilterMaxRecords = 500;

        public static class HealthCheckTags
        {
            public const string Readiness = "readiness";
        }
    }
}
