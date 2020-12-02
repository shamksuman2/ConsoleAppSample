/*       Client Program      */

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Net.Sockets;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TcpClientNs
{
    public class TcpClientWrapper
    {

        public static async Task Main()
        {

            try
            {
                while (true)
                {
                    var msg = GetTelemetryMessages();
                    // Send the message  
                    byte[] bytes = sendMessage(Encoding.Unicode.GetBytes(msg));
                    await Task.Delay(10000);
                }

            }

            catch (Exception e)
            {
                Console.WriteLine(Error.....  + e.StackTrace);
            }
        }

        private static byte[] sendMessage(byte[] messageBytes)
        {
            const int bytesize = 1024 * 1024;
            try // Try connecting and send the message bytes  
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(127.0.0.1, 8001); // Create a new connection  
                NetworkStream stream = client.GetStream();

                stream.Write(messageBytes, 0, messageBytes.Length); // Write the bytes  
                Console.WriteLine(================================);
                Console.WriteLine(=   Connected to the server    =);
                Console.WriteLine(================================);
                Console.WriteLine(Waiting for response...);

                messageBytes = new byte[bytesize]; // Clear the message   

                // Receive the stream of bytes  
                stream.Read(messageBytes, 0, messageBytes.Length);

                // Clean up  
                stream.Dispose();
                client.Close();
            }
            catch (Exception e) // Catch exceptions  
            {
                Console.WriteLine(e.Message);
            }

            return messageBytes; // Return response  
        }

        private static string GetTelemetryMessages()
        {
            double minTemperature = 20;
            Random rand = new Random();
            string[] itemName = { Red Pepper, Black Olives, American Chees, Ham, Jalpenos, Green Papper, Chicken Strips, Becon };
            var telemetrymsgsbody = new List<TelemetryMessageBody>();
            for (int i = 0; i < 8; i++)
            {
                var item = new TelemetryMessageBody
                {
                    Temperature = string.Format({0:0.00}, minTemperature + rand.NextDouble() * 10),
                    ItemId = i.ToString(),
                    ApplicationProperties = new ApplicationProperties { MessageType = FoodItemTemperature },
                    ItemBlobUrl = ,
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

