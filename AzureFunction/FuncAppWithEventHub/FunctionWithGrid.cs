// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.SignalRService;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Console.Entity;

namespace FunctionAppEventHub
{
    public static class FunctionWithGrid
    {
        [FunctionName("Function1")]
        public static async Task Run([EventGridTrigger] EventGridEvent eventGridEvent,
            //[SignalR(HubName = "notifications", ConnectionStringSetting = "AzureSignalRConnectionString")]
            //IAsyncCollector<SignalRMessage> signalRMessages,
            ILogger log)
        {
            
            var griddata = JsonConvert.DeserializeObject<object>(JsonConvert.SerializeObject(eventGridEvent.Data));
            log.LogInformation(JsonConvert.SerializeObject(eventGridEvent.Data) + "Shambhu");
            log.LogInformation(griddata + "Shambhu");
            //string messageBody = Encoding.UTF8.GetString(eventGridEvent.Data);

            //// Replace these two lines with your processing logic.
            //log.LogInformation($"C# Event Hub trigger function processed a message: {messageBody}");
            //await Task.Yield();

            //log.LogInformation($"Sending notification for Sham new order");

            //var foodItems =
            //    JsonConvert.DeserializeObject<TelemetryDetails>(JsonConvert.SerializeObject(eventGridEvent.Data));


            //await signalRMessages.AddAsync(
            //    new SignalRMessage
            //    {
            //        Target = "Transaction",
            //        Arguments = new object[] {foodItems}
            //    });

        }

        [FunctionName("negotiate")]
        public static SignalRConnectionInfo NotificationsSignalRInfo(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")]
            HttpRequest req,
            [SignalRConnectionInfo(HubName = "notifications")]
            SignalRConnectionInfo connectionInfo)
        {
            return connectionInfo;
        }
    }
}
