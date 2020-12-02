using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Dover.DxFleet.Entity;

namespace Dover.Fleet.Entity
{
    [ExcludeFromCodeCoverage]

    public class NotificationTemplate
    {
        public string Id { get; set; }
        public string OrgId { get; set; }
        public AlertCode Alert { get; set; }
        public string Title { get; set; }
        public string EmailMessage { get; set; }
        public string SmsText { get; set; }
        public string AlertClearedEmailMessage { get; set; }
        public string AlertClearedSmsMessage { get; set; }
    }
}
