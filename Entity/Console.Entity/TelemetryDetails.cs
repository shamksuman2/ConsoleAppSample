using System;
using System.Collections.Generic;

namespace Console.Entity
{
    public class DirectMethodObject
    {
        public string Command { get; set; }
        public string deviceid { get; set; }
        public string siteid { get; set; }
        public string OrganizationId { get; set; }
    }

    public class TelemetryDetails
    {
        public List<TelemetryMessageBody> MsgsBody { get; set; }
    }

    public class TelemetryMessageBody
    {
        public string ItemId { get; set; }
        public string TemperatureUnit { get; set; }

        public string Temperature { get; set; }

        public ApplicationProperties ApplicationProperties { get; set; }
    }

    public class ApplicationProperties
    {
        public string MessageType { get; set; }
    }
}
