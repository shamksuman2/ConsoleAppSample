namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using System;
    using System.Collections.Generic;


    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DocumentDbConfigurationOptions : IDocumentDbConfigurationOptions
    {
        public string CollectionName { get; set; }
        public IEnumerable<string> CollectionPartitionKeyPaths { get; set; }
        public string Key { get; set; }
        public string DatabaseName { get; set; }
        public Uri DocumentDb { get; set; }
        public Dictionary<string, string[]> PartitionKeys { get; set; }
        public int Throughput { get; set; }
        public int? TimeToLive { get; set; }
    }
}
