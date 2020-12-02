namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using System.Threading;
    using Microsoft.Azure.Documents.Linq;
    using System.Collections.Generic;
    using Polly;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DocumentDbUtil
    {
        public static Task<IReadOnlyList<T>> AsAsync<T>(this IQueryable<T> query, int max)
        {
            return query.AsDocumentQuery().AsAsync(max);
        }

        public static async Task<IReadOnlyList<T>> AsAsync<T>(this IDocumentQuery<T> query, int max)
        {
            var results = new List<T>();
            int remaining = max;

            while (query.HasMoreResults)
            {
                var res = await query.ExecuteNextAsync<T>();
                results.AddRange(res.Take(Math.Min(res.Count, max == -1 ? res.Count : remaining)));

                if (max == -1)
                    continue;

                remaining -= res.Count;
                if (remaining <= 0)
                    break;
            }
            return results;
        }

        public static Task ForEachAsync<T>(this IQueryable<T> query, Action<T> action, int max)
        {
            return query.AsDocumentQuery().ForEachAsync(action, max);
        }

        public static async Task ForEachAsync<T>(this IDocumentQuery<T> query, Action<T> action, int max)
        {
            int count = 0;
            while (query.HasMoreResults)
            {
                var res = await query.ExecuteNextAsync<T>();
                foreach (var d in res)
                {
                    action(d);
                    if (++count > max)
                        return;
                }
            }
        }

        public static Task ForEachAsync<T>(this IQueryable<T> query, Func<T, Task> action, int max)
        {
            return query.AsDocumentQuery().ForEachAsync(action, max);
        }

        public static async Task ForEachAsync<T>(this IDocumentQuery<T> query, Func<T, Task> action, int max)
        {
            int count = 0;
            while (query.HasMoreResults)
            {
                var res = await query.ExecuteNextAsync<T>();
                foreach (var d in res)
                {
                    await action(d);
                    if (++count > max)
                        return;
                }
            }
        }

        public static IDocumentClient CreateReliableClient(Uri documentDb, string key)
        {
            return new DocumentClient(documentDb, key, new ConnectionPolicy { ConnectionMode = ConnectionMode.Direct, ConnectionProtocol = Protocol.Tcp });
        }

        public static Task<Database> GetOrAddDatabase(string databaseName, IDocumentClient client, CancellationToken token)
        {
            return Policy
                .Handle<DocumentClientException>(e => { return true; })
                .WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(1 << Math.Min(attempt, 3)))
                .ExecuteAsync(async () =>
                {
                    token.ThrowIfCancellationRequested();
                    var query = client.CreateDatabaseQuery().Where(db => db.Id == databaseName).AsDocumentQuery();

                    if (query.HasMoreResults)
                    {
                        var res = await query.ExecuteNextAsync<Database>();
                        if(res.Any())
                            return res.First();
                    }

                    return (await client.CreateDatabaseAsync(new Database { Id = databaseName }).ConfigureAwait(false)).Resource;
                    
                });
        }

        public static async Task<Database> GetDatabase(string databaseName, IDocumentClient client, CancellationToken token)
        {
            while(true)
            {
                token.ThrowIfCancellationRequested();
                var query = client.CreateDatabaseQuery().Where(db => db.Id == databaseName).AsDocumentQuery();

                if (query.HasMoreResults)
                {
                    var res= await query.ExecuteNextAsync<Database>();
                    if (res.Any())
                        return res.First();
                }
                await Task.Delay(1000, token);
            }
        }

        public static Task<DocumentCollection> GetOrAddDocumentCollection(string collectionId, IDocumentClient client, Database database, CancellationToken token, int throughput,int? timeToLive, Func<string, DocumentCollection> factory = null)
        {
            return Policy
                .Handle<DocumentClientException>(e => { return true; })
                .WaitAndRetryForeverAsync(attempt => TimeSpan.FromSeconds(1 << Math.Min(attempt, 3)))
                .ExecuteAsync(async () =>
                {
                    token.ThrowIfCancellationRequested();
                    var query = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).AsDocumentQuery();

                    if (query.HasMoreResults)
                    {
                        var res = await query.ExecuteNextAsync<DocumentCollection>();
                        if (res.Any())
                            return res.First();
                    }

                    var collection = factory == null ? new DocumentCollection { Id = collectionId } : factory(collectionId);


                    collection.DefaultTimeToLive = timeToLive <= 0 ? -1 : timeToLive * 60 * 60 * 24; // expire all documents after timeToLive no of days. -1 indicates never expire

                    throughput =  throughput == 0 ? 400 : throughput;
                    
                    return (await client.CreateDocumentCollectionAsync(database.SelfLink, collection,new RequestOptions { OfferThroughput = throughput }).ConfigureAwait(false)).Resource;
                });
        }

        public static async Task<DocumentCollection> GetDocumentCollection(string collectionId, IDocumentClient client,
            Database database, CancellationToken token)
        {

                DocumentCollection documentCollection = null;
                var query = client.CreateDocumentCollectionQuery(database.SelfLink).Where(c => c.Id == collectionId).AsDocumentQuery();

                if (query.HasMoreResults)
                {
                    var res = await query.ExecuteNextAsync<DocumentCollection>();
                    if (res.Any()) 
                        return res.First();
                    else
                        return documentCollection;
                }
                else
                {
                    return documentCollection;
                }
        }
    }
}
