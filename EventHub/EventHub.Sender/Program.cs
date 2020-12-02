using EventHub.Sender;
using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace SampleEphReceiver
{

    public class Program
    {
        public static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {
            var eventhubSenderWrapper = new EventHubSenderWrapper();
            var messaageObject = new TelemetryData()
            {
                messageclass = "telemetry",
                messagetype = "Fleet",
                devicetype = "Terminal",
                messagebody = new MessageBody() { AlterType = "Printer Out Of Order", SiteName = "001" }
            };
            var message = JsonConvert.SerializeObject(messaageObject);
            for (int i = 0; i < 100; i++)
            {
                await eventhubSenderWrapper.SendAsync(new EventData(Encoding.UTF8.GetBytes(message)));
            }
        }
    }

    public class MessageBody
    {
        public string SiteName { get; set; }
        public string AlterType { get; set; }
    }

    public class TelemetryData
    {
        [JsonProperty(PropertyName = "device-type")]
        public string devicetype { get; set; }

        [JsonProperty(PropertyName = "message-type")]
        public string messagetype { get; set; }

        [JsonProperty(PropertyName = "message-class")]
        public string messageclass { get; set; }

        [JsonProperty(PropertyName = "message-body")]
        public MessageBody messagebody { get; set; }
    }
}