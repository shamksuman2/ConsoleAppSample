namespace Dover.Fleet.Common.CosmoDB.Handler
{
    using Microsoft.Azure.Documents;

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class DocumentDbRepositoryResources
    {
        public string CollectionId => DocumentCollection?.Id;
        public string DatabaseId => DocumentDatabase?.Id;
        internal IDocumentClient DocumentClient { get; set; }
        internal DocumentCollection DocumentCollection { get; set; }
        internal Database DocumentDatabase { get; set; }
    }
}
