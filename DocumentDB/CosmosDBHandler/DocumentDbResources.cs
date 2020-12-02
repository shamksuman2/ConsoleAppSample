namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System.Threading;
    using System.Threading.Tasks;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DocumentDbResources
    {
        /// <summary>
        ///     Creates new database, collection and document client if those required resources do not exist.
        /// </summary>
        /// <param name=configuration>The definition of required resources.</param>
        /// <param name=cancellationToken>The instance of a cancellation token.</param>
        /// <returns>A collection of created or retrieved resources.</returns>
        public static async Task<DocumentDbRepositoryResources> CreateResourcesAsync(
            IDocumentDbConfigurationOptions configuration, CancellationToken cancellationToken)
        {
            var client = new DocumentClient(configuration.DocumentDb, configuration.Key);
            var database = await DocumentDbUtil.GetOrAddDatabase(configuration.DatabaseName, client, cancellationToken);
            var documentCollection = await DocumentDbUtil.GetOrAddDocumentCollection(
                configuration.CollectionName, client, database, cancellationToken, configuration.Throughput, configuration.TimeToLive, id => CreateDocumentCollection(configuration));
            var resources = new DocumentDbRepositoryResources
            {
                DocumentClient = client,
                DocumentDatabase = database,
                DocumentCollection = documentCollection
            };
            return resources;
        }

        internal static DocumentDbRepositoryResources CreateResources(IDocumentDbConfiguration configuration,
            string collectionName)
        {
            var options = new DocumentDbConfigurationOptions
            {
                DatabaseName = configuration.DatabaseName,
                CollectionName = collectionName,
                Key = configuration.Key,
                DocumentDb = configuration.DocumentDb,
                CollectionPartitionKeyPaths = configuration.PartitionKeys.ContainsKey(collectionName) == true ? configuration.PartitionKeys[collectionName] : default,
                Throughput = configuration.Throughput,
                TimeToLive = configuration.TimeToLive
            };
            return CreateResources(options);
        }

        internal static DocumentDbRepositoryResources CreateResources(IDocumentDbConfigurationOptions configuration)
        {
            var resources = CreateResourcesAsync(configuration, CancellationToken.None)
                .ConfigureAwait(false).GetAwaiter().GetResult();
            return resources;
        }

        private static DocumentCollection CreateDocumentCollection(IDocumentDbConfigurationOptions configuration)
        {
            var collection = new DocumentCollection { Id = configuration.CollectionName };

            if (configuration.CollectionPartitionKeyPaths != null)
                foreach (var partitionKey in configuration.CollectionPartitionKeyPaths)
                    collection.PartitionKey.Paths.Add(partitionKey);

            return collection;
        }
    }
}
