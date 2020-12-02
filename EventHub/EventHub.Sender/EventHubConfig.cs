using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace EventHub.Sender
{
    public class EventHubConfig
    {
        private static readonly Lazy<EventHubConfig> _instance =
            new Lazy<EventHubConfig>(() => new EventHubConfig());

        private EventHubConfig()
        { }

        public static string EventHubName { get; set; }
        public static string ReadConnectiongString { get; set; }
        public static string WriteConnectionString { get; set; }
        public static string StorageContainerName { get; set; }
        public static string StorageConnectionString { get; set; }
        public static string ConsumerGroupName { get; set; }

        public static EventHubConfig EventHubConfigProvider
        {
            get
            {
                IConfiguration config = new ConfigurationBuilder()
                   .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddEnvironmentVariables()
                   .Build();

                EventHubName = config.GetSection("EventHubConfig:EventHubName").Value;
                ReadConnectiongString = config.GetSection("EventHubConfig:ReadConnectiongString").Value;
                WriteConnectionString = config.GetSection("EventHubConfig:WriteConnectionString").Value;
                StorageContainerName = config.GetSection("EventHubConfig:StorageContainerName").Value;
                StorageConnectionString = config.GetSection("EventHubConfig:StorageConnectionString").Value;
                ConsumerGroupName = config.GetSection("EventHubConfig:ConsumerGroupName").Value;
                return _instance.Value;
            }
        }

    }
}
