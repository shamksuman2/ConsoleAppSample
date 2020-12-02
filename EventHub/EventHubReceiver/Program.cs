using Microsoft.Azure.EventHubs;
using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SampleEphReceiver
{

    public class Program
    {
        //Device online/offline
        //private const string EventHubConnectionString = "Endpoint=sb://omtesteventhub.servicebus.windows.net/;SharedAccessKeyName=mypolicy;SharedAccessKey=SIcEW9bMr//4JQxSABrtW/LwoNnE+dTLbc71JI/5S+0=;";
        //private const string EventHubName = "omtesteventhub";
        //private const string EventHubConsumerGroupName = "$Default";

        private const string EventHubConnectionString = "Endpoint=sb://ceres-qa.servicebus.windows.net/;SharedAccessKeyName=DxFleetDev;SharedAccessKey=uyrskhC+7cHnes+PSbi0qLwkQOlSAG0qlcA34/WTDUY=;";
        private const string EventHubName = "notifications-messages";
        private const string EventHubConsumerGroupName = "$Default";
        private const string StorageContainerName = "siteonboarding-eh-container";
        private const string StorageAccountName = "stcommondxfleetdev";
        private const string StorageAccountKey = "nUTRbjY2b3KEm7LdFBcmRPg/65wHdQV2bt3A3KkLb0U1yPWcPU6VJkSz/iDYk76ELz06Rr90ZnD1Yw4hK5DkXA==";
        //DefaultEndpointsProtocol=https;AccountName=stcommondxfleetdev;AccountKey=nUTRbjY2b3KEm7LdFBcmRPg/65wHdQV2bt3A3KkLb0U1yPWcPU6VJkSz/iDYk76ELz06Rr90ZnD1Yw4hK5DkXA==;EndpointSuffix=core.windows.net

        //private const string EventHubConnectionString = "Endpoint=sb://ceres-qa.servicebus.windows.net/;SharedAccessKeyName=FleetListener;SharedAccessKey=ckh1qDSHtXx3LvKthDuLnZUXn8W3XK6IkaSf8gSuTzw=;";
        //private const string EventHubName = "notifications-messages";
        //private const string EventHubConsumerGroupName = "fleet-qa-cg";
        //private const string StorageContainerName = "dataservice-eh-deviceonlineoffline";
        //private const string StorageAccountName = "blobcommonfleetqa";
        //private const string StorageAccountKey = "HXQ9EsPXK0NsdvOZTr+xizE6iN0uTHjYAMEZb812ERWRnlkxzez/9WAaCTFTE+8NPC6tFxFZ4OqRmHXaXy8oUw==";

        //Receive dataservice telemetry msg 
        //private const string EventHubConnectionString = "Endpoint=sb://ehns-fleet-qa2.servicebus.windows.net/;SharedAccessKeyName=ListenOnlyKey;SharedAccessKey=PRvTBdi8odhjcLfRsNH8IgVTtKkivNUS1QRj+k1G37E=";
        //private const string EventHubName = "eh-ds-telemetry-fleet-qa2";
        //private const string EventHubConsumerGroupName = "msgprocessor-cg";
        //private const string StorageContainerName = "dataservice-eh-messageprocessor";
        //private const string StorageAccountName = "blobcommonfleetqa2";
        //private const string StorageAccountKey = "Jfc0d3JlxRe2vpXTIMhfIIZ7Jj3hZvvJfCiV20tfCO3kIhkKEItmGdc48Mf1iwO1167jwTY9SJ7ato7fNmFgQA==";

        //private const string EventHubConnectionString = "Endpoint=sb://ehns-fleet-qa.servicebus.windows.net/;SharedAccessKeyName=ListenOnlyKey;SharedAccessKey=FbL14elGaA483riwo6rKTqywebOjskrXJBCI3fdXyuc=;";
        //private const string EventHubName = "eh-ds-telemetry-fleet-qa";
        //private const string StorageContainerName = "transactions-container";
        //private const string StorageAccountName = "stcommondxfleetdev";
        //private const string StorageAccountKey = "TShCj1DUY+JS/I13dn7UVzprM3GqYsxNr0Iq8Cb8cxRk6n5ecuYm17qBcx/0RPvoz3N+6CAzZH9/dJwJRo5uOA==";
        //private const string EventHubConsumerGroupName = "msgprocessor-cg-dxfleet-dev";

        //private const string EventHubConnectionString = "Endpoint=sb://ehns-fleet-dev.servicebus.windows.net/;SharedAccessKeyName=ListenOnlyKey;SharedAccessKey=+2CcybFSdyAivc5fE1xmJ/RwV7xf1YvuHGQcYcE4NfA=";
        //private const string EventHubName = "eh-ds-telemetry-fleet-dev";
        //private const string StorageContainerName = "ds-eh-msgprocessor-dev";
        //private const string StorageAccountName = "blobcommonfleetdev";
        //private const string StorageAccountKey = "RjxKFyb8yr99GhufN/KOLFoVCT4fGlBwLdJ8r2n/yU7iEAiI7PWsdhrSQhT1OWdL3NF5u4LBslfvlQIcEcf1mA==";
        //private const string EventHubConsumerGroupName = "$Default";



        private static readonly string StorageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", StorageAccountName, StorageAccountKey);

        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            Console.WriteLine("Registering EventProcessor...");

            var eventProcessorHost = new EventProcessorHost(
                EventHubName,
                EventHubConsumerGroupName,
                EventHubConnectionString,
                StorageConnectionString,
                StorageContainerName);

            // Registers the Event Processor Host and starts receiving messages
            await eventProcessorHost.RegisterEventProcessorAsync<SimpleEventProcessor>();

            Console.WriteLine("Receiving. Press ENTER to stop worker.");
            Console.ReadLine();

            // Disposes of the Event Processor Host
            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }

    public class SimpleEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.WriteLine($"Processor Shutting Down. Partition '{context.PartitionId}', Reason: '{reason}'.");
            return Task.CompletedTask;
        }

        public Task OpenAsync(PartitionContext context)
        {
            Console.WriteLine($"SimpleEventProcessor initialized. Partition: '{context.PartitionId}'");
            return Task.CompletedTask;
        }

        public Task ProcessErrorAsync(PartitionContext context, Exception error)
        {
            Console.WriteLine($"Error on Partition: {context.PartitionId}, Error: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            foreach (var eventData in messages)
            {
                eventData.Properties.TryGetValue("message-type", out var MessageType);
                eventData.Properties.TryGetValue("alert-type", out var alertType);
                eventData.Properties.TryGetValue("siteId", out var siteId);
                eventData.Properties.TryGetValue("id", out var deviceId);
                eventData.Properties.TryGetValue("device-type", out var deviceType);

                var data = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                var data1 = JsonConvert.DeserializeObject<Root>(data);
                var receivedmessage =
                    $"DateTime {DateTime.Now.ToString()}, Property-MessageType :{MessageType} Alert-Type : {alertType}, site: '{siteId}', deviceId :{deviceId}, devicetype :{deviceType}\n" + data;
                var path = @"D:\Assignment\ConsoleApp\EventHub\EventHubReceiver\EventHubListernData.txt";
                WriteToFile(path, receivedmessage);
            }
            return context.CheckpointAsync();
        }

        private void WriteToFile(string path, string data)
        {
            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(data + Environment.NewLine + "-------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
    public class Metadata
    {
        public string deviceId { get; set; }
        public string deviceType { get; set; }
        public string siteId { get; set; }
        public string dashboardAbsoluteUrl { get; set; }
        public string deviceAbsoluteUrl { get; set; }
    }

    public class Root
    {
        public string id { get; set; }
        public string messageType { get; set; }
        public string contact { get; set; }
        public string description { get; set; }
        public List<string> fileUrls { get; set; }
        public string name { get; set; }
        public string notificationId { get; set; }
        public Metadata metadata { get; set; }
        public int priority { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public DateTime triggerTime { get; set; }
        public string userId { get; set; }
        public object messageTemplateId { get; set; }
        public object product { get; set; }
        public int version { get; set; }
    }


}