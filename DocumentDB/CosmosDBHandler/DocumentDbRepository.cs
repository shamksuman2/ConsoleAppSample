namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using Microsoft.Azure.Documents.Linq;
    using System.Threading;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DocumentDbRepository<T>
        : IAsyncRepository<T>
    {
        private readonly string cosmosDatabase;
        private readonly string database;
        private readonly IDocumentClient @object;

        internal IDocumentClient Client { get; }
        internal DocumentCollection DocumentCollection { get; }

        public DocumentDbRepository(IDocumentDbConfiguration configuration)
        {
            var resources = DocumentDbResources.CreateResources(configuration, typeof(T).Name);
            Client = resources.DocumentClient;            
            DocumentCollection = resources.DocumentCollection;
            database = configuration.DatabaseName;
        }


        public async Task<bool> CreateDatabase(string databasename)
        {
            var response = await Client.CreateDatabaseAsync(new Database { Id = databasename });
            return response?.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> CreateNewDocumentCollection(string databasename,string collectionName,string partitionkey)
        {

            DocumentCollection docCollection = new DocumentCollection();
            docCollection.Id = collectionName;
            docCollection.PartitionKey.Paths.Add("/" + partitionkey);
            var response = await Client.CreateDocumentCollectionAsync(UriFactory.CreateDatabaseUri(databasename), docCollection);
            return response?.StatusCode == HttpStatusCode.Created;
        }

        
        public async Task<bool> CreateNewDocument(string databasename, string collectionName, T document)
        {
            DocumentCollection docCollection = new DocumentCollection();
            docCollection.Id = collectionName;
            var response=await Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databasename, collectionName),document);
            return response?.StatusCode == HttpStatusCode.Created;
        }

     
        

        public async Task<bool> Add(T document)
        {

            var response = await Client.CreateDocumentAsync(DocumentCollection.SelfLink, document).ConfigureAwait(false);
            return response?.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Upsert(T document)
        {            
            var response = await Client.UpsertDocumentAsync(DocumentCollection.SelfLink, document).ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
        }


        public async Task<bool> DeleteDocument(string id)
        {
            var document = await GetDocumentByIdAsync(id);
            var response = await Client.DeleteDocumentAsync(document.SelfLink).ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        public async Task<bool> DeleteDocument(string id,string partitionkey)
        {           
            var response = await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(database, DocumentCollection.Id, id),new RequestOptions { PartitionKey = new PartitionKey(partitionkey) });
            return response.StatusCode == HttpStatusCode.NoContent;
        }

        /// <inheritdoc/>
        public async Task<bool> DeleteDocument(string id, int? secondsToLive)
        {
            var document = await GetDocumentByIdAsync(id);
            var removedDocument = CreateEmptyReplacementDocument(id, secondsToLive);
            var response = await Client.ReplaceDocumentAsync(document.SelfLink, removedDocument).ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.OK;
        }



        public async Task<bool> AddOrUpdateProperties(Guid id, params DocumentUpdate[] updates)
        {
            var idString = id.ToString();
            var document = Client.CreateDocumentQuery(DocumentCollection.SelfLink).Where(d => d.Id == idString).AsEnumerable().FirstOrDefault();

            if (document == null)
                document = new Document() { Id = idString };

            foreach (var update in updates)
            {
                document.SetPropertyValue(update.Name, update.Value);
            }

            var response = await Client.UpsertDocumentAsync(DocumentCollection.SelfLink, document).ConfigureAwait(false);
            return response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created;
        }

        public async Task<bool> Any(Expression<Func<T, bool>> predicate = null)
        {
            var query = Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink)
                .Where(predicate ?? (x => true))
                .AsDocumentQuery();

            var result = new List<T>();
            while (query.HasMoreResults)
            {
                var currentCount = result.Count;
                var clients = await query.ExecuteNextAsync<T>().ConfigureAwait(true);

                if (clients.Count() > 0)
                {
                    return true;
                }
                if (result.Count == currentCount)
                    break;
            }
            return false;
        }

       

        

        public async Task<IList<T>> Find(Expression<Func<T, bool>> predicate = null)
        {
            var option = new FeedOptions{ EnableCrossPartitionQuery = true};
            var query = Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink, option)
                .Where(predicate ?? (x => true))
                .AsDocumentQuery();

            var result = new List<T>();
            while (query.HasMoreResults)
            {
                var currentCount = result.Count;
                var clients = await query.ExecuteNextAsync<T>().ConfigureAwait(true);
                result.AddRange(clients);
                if (result.Count == currentCount)
                    break;
            }

            return result;
        }


        public async Task<IList<T>> Find(string sqlQuery)
        {
            var query = Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink, sqlQuery).AsDocumentQuery();

            var result = new List<T>();
            while (query.HasMoreResults)
            {
                var currentCount = result.Count;
                var clients = await query.ExecuteNextAsync<T>().ConfigureAwait(true);
                result.AddRange(clients);
                if (result.Count == currentCount)
                    break;
            }

            return result;
        }

        public async Task<IList<T>> FindWithCrossPartitionEnabled(string sqlQuery)
        {
            var option = new FeedOptions {EnableCrossPartitionQuery = true};
            var query = Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink, sqlQuery, option).AsDocumentQuery();

            var result = new List<T>();
            while (query.HasMoreResults)
            {
                var currentCount = result.Count;
                var clients = await query.ExecuteNextAsync<T>().ConfigureAwait(true);
                result.AddRange(clients);
                if (result.Count == currentCount)
                    break;
            }

            return result;
        }

        public Task<T> FindFirst(Expression<Func<T, bool>> predicate = null) =>
            Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink)
                .Where(predicate ?? (x => true))
                .AsDocumentQuery()
                .First();


        public Task<T> FindFirstOrDefault(Expression<Func<T, bool>> predicate = null) => 
            Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink)
                .Where(predicate ?? (x => true))
                .AsDocumentQuery()
                .FirstOrDefault();


        public Task<T> FindFirstOrDefault(string sqlQuery) =>
            Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink, sqlQuery)
                .AsDocumentQuery()
                .FirstOrDefault();

        



        

        public DocumentDbRepository(string cosmosDatabase, IDocumentDbConfiguration configuration)
        {
            var resources = DocumentDbResources.CreateResources(configuration, typeof(T).Name);

            Client = resources.DocumentClient;
            DocumentCollection = resources.DocumentCollection;
        }

        public DocumentDbRepository(IDocumentDbConfigurationOptions configuration)
        {
            var resources = DocumentDbResources.CreateResources(configuration);

            Client = resources.DocumentClient;
            DocumentCollection = resources.DocumentCollection;
        }

        private DocumentDbRepository(IDocumentClient client, DocumentCollection collection)
        {
            Client = client;
            DocumentCollection = collection;
        }

        public DocumentDbRepository(string cosmosDatabase, IDocumentClient @object)
        {
            this.cosmosDatabase = cosmosDatabase;
            this.@object = @object;
        }
        

        

        private static Document CreateEmptyReplacementDocument(string id, int? secondsToLive)
        {
            var removed = new Document();
            removed.SetPropertyValue(id, id);
            removed.TimeToLive = secondsToLive;
            removed.SetPropertyValue("isDeleted", true);
            return removed;
        }

        

        
        private async Task<Document> GetDocumentByIdAsync(string id)
        {
            var document = await Client.CreateDocumentQuery(DocumentCollection.SelfLink, new FeedOptions { EnableCrossPartitionQuery = true })
                .Where(x => x.Id == id)
                .AsDocumentQuery()
                .FirstOrDefault();

            if (document == null)
            {
                throw new NullReferenceException($"Could not find document {id} to delete");
            }

            return document;
        }
        

        
        public static async Task<IAsyncRepository<T>> Create(Uri documentDb, string key, string databaseName,
           string documentCollection, CancellationToken token)
        {
            var configuration = new DocumentDbConfigurationOptions
            {
                DatabaseName = databaseName,
                CollectionName = documentCollection,
                Key = key,
                DocumentDb = documentDb
            };
            var resources = await DocumentDbResources.CreateResourcesAsync(configuration, token);
            return new DocumentDbRepository<T>(resources.DocumentClient, resources.DocumentCollection);
        }
    }
}
