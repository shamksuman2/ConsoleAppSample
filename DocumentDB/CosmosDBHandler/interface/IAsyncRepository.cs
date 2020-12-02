namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public interface IAsyncRepository<T>     
    {
        Task<IList<T>> Find(Expression<Func<T, bool>> predicate = null);

        Task<IList<T>> Find(string sqlQuery);

        Task<IList<T>> FindWithCrossPartitionEnabled(string sqlQuery);

        Task<T> FindFirst(Expression<Func<T, bool>> predicate = null);

        Task<T> FindFirstOrDefault(Expression<Func<T, bool>> predicate = null);

        Task<T> FindFirstOrDefault(string sqlQuery);

        Task<bool> Add(T document);

        Task<bool> Upsert(T document);

        Task<bool> AddOrUpdateProperties(Guid id, DocumentUpdate[] updates);

        Task<bool> DeleteDocument(string id);

        Task<bool> DeleteDocument(string id, string partitionkey);

        /// <summary>
        /// Clears all properties from current document and sets time in seconds after which document will be physically deleted.
        /// </summary>
        /// <param name=id>Identifier of document that will be deleted.</param>
        /// <param name=secondsToLive>Time in seconds after which document will be deleted physically.</param>
        /// <returns>Value indicating whether delayed removal was successful.</returns>
        /// <remarks>CosmosDb settings for collection must have TTL enabled in order to delete document after expitation date is reached.</remarks>
        Task<bool> DeleteDocument(string id, int? secondsToLive);

        Task<bool> CreateDatabase(string databasename);

        Task<bool> CreateNewDocumentCollection(string databasename, string collectionName, string partitionkey);

        Task<bool> CreateNewDocument(string databasename, string collectionName, T document);

        Task<bool> Any(Expression<Func<T, bool>> predicate = null);

    }
}
