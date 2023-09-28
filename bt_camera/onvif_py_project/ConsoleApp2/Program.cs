using System;
using System.Threading.Tasks;
using Onvif;

namespace OnvifExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ONVIFCamera mycam = new ONVIFCamera("192.168.88.153", 80, "service", "Bosch$123");

            var eventService = mycam.CreateEventsService();
            var eventProperties = await eventService.GetEventPropertiesAsync();
            Console.WriteLine(eventProperties);

            var pullpoint = mycam.CreatePullpointService();
            var pullMessagesRequest = pullpoint.CreateType<PullMessages>();
            pullMessagesRequest.MessageLimit = 100;
            var pullMessagesResponse = await pullpoint.PullMessagesAsync(pullMessagesRequest);
            Console.WriteLine(pullMessagesResponse);
        }
    }
}