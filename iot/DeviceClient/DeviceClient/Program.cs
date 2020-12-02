using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DeviceClientSample
{
    class Program
    {
        private readonly static string s_deviceConnectionString = "HostName=foodtemperaturepociot.azure-devices.net;DeviceId=foottempdevice1;SharedAccessKey=HfXDM2Aimu/tUevJlVcODaUCZ3YVEYkldf1Xf/GULkw=";
        private static TransportType s_transportType = TransportType.Amqp;

        public static async Task Main(string[] args)
        {
            using (var deviceClient = DeviceClient.CreateFromConnectionString(s_deviceConnectionString, s_transportType))
            {
                await deviceClient
               .SetMethodHandlerAsync("GetDeviceCommand", ReceiveDeviceCommandAsync, null).ConfigureAwait(false);

                await SendDeviceToCloudMessagesAsync(deviceClient);//.ConfigureAwait(false)
            }
            // Setup a callback for the 'GetDeviceName' method.



            Console.WriteLine("Done.\n");
            Console.ReadLine();
        }

        private static async Task SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient)
        {
            try
            {
                while (true)
                {
                    string messageString = GetTelemetryMessages();

                    var message = new Message(Encoding.ASCII.GetBytes(messageString));
                    await s_deviceClient.SendEventAsync(message);
                    Console.WriteLine("{0} > Sending message: {1}, DateTime.Now, messageString");
                    await Task.Delay(200 * 5);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetTelemetryMessages()
        {
            double minTemperature = 20;
            Random rand = new Random();
            string[] itemName = { "Red Pepper", "Black Olives", "American Chees", "Ham", "Jalpenos", "Green Papper", "Chicken Strips", "Becon" };
            var telemetrymsgsbody = new List<TelemetryMessageBody>();
            for (int i = 0; i < 8; i++)
            {
                var item = new TelemetryMessageBody
                {
                    Temperature = string.Format("{0:0.00}",minTemperature + rand.NextDouble() * 10),
                    ItemId = i.ToString(),
                    ApplicationProperties = new ApplicationProperties { MessageType = "FoodItemTemperature" },
                    ItemName = itemName[i]
                };
                telemetrymsgsbody.Add(item);
            }
            var itemcollection = new TelemetryDetails { MsgsBody = telemetrymsgsbody };

            //MsgBody = new FSCMessageBody
            //{
            //    TransSite = ConfigFileInfo.SiteId,
            //    TransDetails = objStringWriter.ToString(),
            //    TransMerchantID = ConfigFileInfo.MerchantID,
            //    DeviceId = ConfigFileInfo.DeviceId,
            //    ApplicationProperties = new ApplicationProperties
            //    {
            //        MessageType = TransactionDetails
            //    }
            //}

            return JsonConvert.SerializeObject(itemcollection);
        }

    private static Task<MethodResponse> ReceiveDeviceCommandAsync(MethodRequest methodRequest, object userContext)
    {
        Console.WriteLine($"\t *** {methodRequest.Name} was called.");

        MethodResponse retValue;
        if (userContext == null)
        {
            retValue = new MethodResponse(new byte[0], 500);
        }
        else
        {
            var data = JsonConvert.DeserializeObject<DirectMethodObject>(Encoding.UTF8.GetString(methodRequest.Data));
            string result = JsonConvert.SerializeObject(data);
            retValue = new MethodResponse(Encoding.UTF8.GetBytes(result), 200);
        }

        return Task.FromResult(retValue);
    }
}

public class DirectMethodObject
{
    public string Command { get; set; }
    public string deviceid { get; set; }
    public string siteid { get; set; }
    public string OrganizationId { get; set; }
}

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
