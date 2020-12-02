using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServiceBusQueueSender
{
    class Program
    {
        //const string connectionString = Endpoint=sb://sbns-fleet-dev.servicebus.windows.net/;SharedAccessKeyName=SendOnlyKey;SharedAccessKey=38Ir9V03Ju/mZpeAjOnMz+hdvoe4PRsElOgnWyQg2qo=;
        const string connectionString = "Endpoint=sb://sbns-fleet-qa.servicebus.windows.net/;SharedAccessKeyName=SendOnlyKey;SharedAccessKey=n2X3vM7Xs+cH8NNv6UBQD93cJmcL/VbnEGZSTBcK7NM=";
        const string queueName = "commandmanager-queue";

        private static IQueueClient _queueClient = new QueueClient(connectionString, queueName);

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        private static async Task MainAsync(string[] args)
        {

            var message = DeviceDirectMehodMessage();

            var quemessage = new Message(Encoding.UTF8.GetBytes(message));
            quemessage.MessageId = new Guid().ToString();

            await _queueClient.SendAsync(quemessage);
        }

        private static string DeviceDirectMehodMessage()
        {
            var directMethodObjects = new List<DirectMethodObject>() 
            {
                new DirectMethodObject() {
                    Command = "CardUpdate",
                    deviceid="02f087a1-c8e1-0c88-aadb-3fcfbbcf836e",
                    MessageType="devicecommand",
                    OrganizationId="a18943db-7236-4338-9e2f-76d05bd13d60",
                    siteid="505",
                    OnboardingSiteId="2f8a8412-5f39-428a-84d4-b9073eff2146"} 
            };

            var c2dmessage = new C2DMessage() { IndexName = IndexType.command.ToString(), Documents = directMethodObjects };

            return JsonConvert.SerializeObject(c2dmessage);
        }
    }

    public class C2DMessage
    {
        public string IndexName { get; set; }
        public List<DirectMethodObject> Documents { get; set; }
    }

    public class DirectMethodObject
    {
        public string Command { get; set; }
        public string deviceid { get; set; }
        public string siteid { get; set; }
        public string OrganizationId { get; set; }
        public string OnboardingSiteId { get; set; }

        public string MessageType { get; set; }

    }

    public enum IndexType
    {
        command
    }
}
