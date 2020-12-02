using System.Collections.Generic;

namespace Dover.Fleet.Common.Configuration
{
    public class CosmosDbConfig
    {
        public string DbKey { get; set; }
        public string DbName { get; set; }
        public string DbEndPoint { get; set; }
        public int Throughput { get; set; }
        public int? TimeToLive { get; set; }
        public List<Collections> Collections { get; set; }
        public string GetConnectionString()
        {
            return "AccountEndpoint=" + DbEndPoint + ";AccountKey=" + DbKey + ";Database=" + DbName;
        }
    }

    public class Collections
    {
        public string Name { get; set; }
        public int Throughput { get; set; }
        public int TimeToLive { get; set; }
        public List<string> PartitionKeys { get; set; }

    }
}
