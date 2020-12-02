using Dover.Fleet.Common.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dover.Fleet.Common.CosmoDB.Handler
{
    public class ServiceRepository : IDocumentDbConfiguration
    {
        public Uri DocumentDb { get; }
        public string Key { get; }
        public string DatabaseName { get; set; }
        public Dictionary<string, string[]> PartitionKeys { get; set; }
        public int Throughput { get; set; }
        public int? TimeToLive { get; set; }

        public ServiceRepository(IOptions<CosmosDbConfig> cosmosDbSettings)
        {
            DocumentDb = new Uri(cosmosDbSettings.Value.DbEndPoint);
            Key = cosmosDbSettings.Value.DbKey;
        }

    }
}
