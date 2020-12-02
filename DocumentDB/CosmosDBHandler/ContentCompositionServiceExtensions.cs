using Dover.Fleet.Common.Configuration;
using Dover.Fleet.CosmoDB.Handler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Dover.Fleet.ContentCompositionService.Reader
{
    [ExcludeFromCodeCoverage]
    public static class ContentCompositionServiceExtensions
    {
        public static void AddRepository(this IServiceCollection services, IOptions<CosmosDbConfig> cosmosDbSettings)
        {
            services.AddSingleton(typeof(IAsyncRepository<Site>), Create<Site>(GetConfiguration(cosmosDbSettings, nameof(Site))));
            //services.AddSingleton(typeof(IAsyncRepository<NotificationContent>), Create<NotificationContent>(GetConfiguration(cosmosDbSettings, nameof(NotificationContent))));
        }

        private static IDocumentDbConfiguration GetConfiguration(IOptions<CosmosDbConfig> cosmosDbSettings, string collectionName)
        {
            var collectionSetting = cosmosDbSettings.Value.Collections.Where(t => t.Name == collectionName).FirstOrDefault();
            var dbconfiguration = new ServiceRepository(cosmosDbSettings);
            dbconfiguration.DatabaseName = cosmosDbSettings.Value.DbName;
            dbconfiguration.Throughput = collectionSetting.Throughput;
            dbconfiguration.PartitionKeys = new Dictionary<string, string[]>();
            if (collectionSetting.PartitionKeys?.Any() == true)
                dbconfiguration.PartitionKeys.Add(collectionName, collectionSetting.PartitionKeys.ToArray());

            return dbconfiguration;
        }

        public static IAsyncRepository<T> Create<T>(IDocumentDbConfiguration dbConfiguration)
        {
            return new DocumentDbRepository<T>(dbConfiguration);
        }
    }
}
