using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dover.DxFleet.Entity
{
    public class Alert
    {
        public string Id { get; set; }
        public string OrgId { get; set; }
        public string SiteId { get; set; }
        public List<AlertSetup> AlertSetups { get; set; }
    }

    public class AlertSetup
    {
        public bool IsAlertEnabled { get; set; }
        public char AlertCode { get; set; }
        public string AlertDevice { get; set; }

        public string AlertName { get; set; }
        public string UserGroup { get; set; }
        public bool IsEmailEnabled { get; set; }
        public bool IsSmsEnabled { get; set; }
        public int SnoozeDuration { get; set; }
    }
}
