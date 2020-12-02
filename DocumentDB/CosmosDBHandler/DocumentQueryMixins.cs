namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Azure.Documents.Linq;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class DocumentQueryMixins
    {
        public static async Task<T> Single<T>(this IDocumentQuery<T> query)
        {
            var response = await query.ExecuteNextAsync<T>().ConfigureAwait(false);
            if (response.Count == 1 && !query.HasMoreResults)
            {
                return response.Single();
            }

            throw new InvalidOperationException($"Expected exactly one matching result, but found {response.Count}.");
        }

        public static async Task<T> First<T>(this IDocumentQuery<T> query)
        {
            var response = await query.ExecuteNextAsync<T>().ConfigureAwait(false);
            return response.First();
        }

        public static async Task<T> FirstOrDefault<T>(this IDocumentQuery<T> query)
        {
            var response = await query.ExecuteNextAsync<T>().ConfigureAwait(false);
            return response.Count >= 1 ? response.First() : default(T);
        }
    }
}
