using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dover.DxFleet.Entity
{
    public class ContactGroup
    {

        public string Id { get; set; }
        public string OrgId { get; set; }
        public string ContactGroupName { get; set; }
        public string Name { get; set; }
        public List<ContactList> Contacts { get; set; }
    }
    public class ContactList
    {

        public string Id { get; set; }
        public string OrgId { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
    }
}
