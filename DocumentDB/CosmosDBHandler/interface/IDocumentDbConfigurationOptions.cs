using System;
using System.Collections.Generic;
using System.Text;

namespace Dover.Fleet.Common.CosmoDB.Handler
{
        public interface IDocumentDbConfigurationOptions : IDocumentDbConfiguration
        {
            string CollectionName { get; }
            IEnumerable<string> CollectionPartitionKeyPaths { get; }
        }
}
