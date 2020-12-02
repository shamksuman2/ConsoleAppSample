using System;
using System.Dynamic;
using Newtonsoft.Json;

namespace Dover.DxFleet.Entity
{
    public class AlertCode
    {

            public string Id { get; set; }
            public string OrgId { get; set; }
            public char Code { get; set; }
            public string AlertName { get; set; }
            public string AlertDevice { get; set; }
    }
}
