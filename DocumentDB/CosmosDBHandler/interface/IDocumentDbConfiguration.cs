using System;
using System.Collections.Generic;

namespace Dover.Fleet.Common.CosmoDB.Handler
{
    public interface IDocumentDbConfiguration
    {
        Uri DocumentDb { get; }
        string Key { get; }
        string DatabaseName { get; }
        Dictionary<string, string[]> PartitionKeys { set; get; }
        int Throughput { set; get; }
        int? TimeToLive { set; get; }

    }
}
