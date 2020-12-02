using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.EventHubs;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionAppEventHub
{
    public class Function
    {
        [FunctionName("EventHubTriggerCSharp")]
        public async Task Run([EventHubTrigger("%AzureEventHubName%", Connection = "AzureEventHubConnectionString", ConsumerGroup ="%AzureEventHubCG%")]
            EventData[] events,
            [SignalR(HubName = "%AzureSignalRName%", ConnectionStringSetting = "AzureSignalRConnectionString")]
            IAsyncCollector<SignalRMessage> signalRMessages, ILogger log)
        {
            var exceptions = new List<Exception>();

            foreach (EventData eventData in events)
            {
                try
                {
                    string messageBody = Encoding.UTF8.GetString(eventData.Body.Array, eventData.Body.Offset, eventData.Body.Count);

                    //// Replace these two lines with your processing logic.
                    //log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
                    //await Task.Yield();

                    log.LogInformation($"Sending notification for Sham new order");

                    var foodItems = JsonConvert.DeserializeObject<TelemetryDetails>(messageBody);

                    if (foodItems != null && foodItems.MsgsBody != null)
                    {
                        await signalRMessages.AddAsync(
                            new SignalRMessage
                            {
                                Target = "Transaction",
                                Arguments = new object[] {foodItems}
                            });

                    }

                    //await signalRMessages.AddAsync(
                    //    new SignalRMessage
                    //    {
                    //        Target = "ShamProductOrdered",
                    //        Arguments = new[] { "Sham New Order" }
                    //    });

                }
                catch (Exception e)
                {
                    // We need to keep processing the rest of the batch - capture this exception and continue.
                    // Also, consider capturing details of the message that failed processing so it can be processed again later.
                    exceptions.Add(e);
                }
            }

            // Once processing of the batch is complete, if any messages in the batch failed processing throw an exception so that there is a record of the failure.

            if (exceptions.Count > 1)
                throw new AggregateException(exceptions);

            if (exceptions.Count == 1)
                throw exceptions.Single();
        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo NotificationsSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest req,
            [SignalRConnectionInfo(HubName ="%AzureSignalRName%" )]
            SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
    //public static class PlaceOrder
    //{
    //    [FunctionName("negotiate")]
    //    public static SignalRConnectionInfo NotificationsSignalRInfo(
    //       [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequest req,
    //       [SignalRConnectionInfo(HubName = "notifications")] SignalRConnectionInfo connectionInfo)
    //    {
    //        return connectionInfo;
    //    }
    //}
    public class TelemetryDetails
    {
        public List<TelemetryMessageBody> MsgsBody { get; set; }
    }

    public class TelemetryMessageBody
    {
        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public string Temperature { get; set; }

        public string ItemBlobUrl { get; set; }

        public ApplicationProperties ApplicationProperties { get; set; }
    }

    public class ApplicationProperties
    {
        public string MessageType { get; set; }
    }
}
