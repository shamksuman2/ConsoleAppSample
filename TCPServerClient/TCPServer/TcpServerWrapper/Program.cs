/*   Server Program    */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Net.Mail;
using Console.Entity;
using Newtonsoft.Json;
using Microsoft.Azure.Devices.Client;

namespace TcpServerNs
{
    class Program
    {
        private readonly static string s_deviceConnectionString = HostName=foodtemperaturepociot.azure-devices.net;DeviceId=foottempdevice1;SharedAccessKey=HfXDM2Aimu/tUevJlVcODaUCZ3YVEYkldf1Xf/GULkw=;
        private static TransportType s_transportType = TransportType.Amqp;
        private static readonly DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(s_deviceConnectionString, s_transportType);


        public static async Task Main(string[] args)
        {

            //Initialize device client

            IPEndPoint ep = new IPEndPoint(IPAddress.Loopback, 8001);
            TcpListener listener = new TcpListener(ep);
            DeviceToClude deviceToClude = new DeviceToClude();

            listener.Start();

            Console.WriteLine(@  
            ===================================================  
                   Started listening requests at: {0}:{1}  
            ===================================================,
            ep.Address, ep.Port);

            // Run the loop continously; this is the server.  
            while (true)
            {
                const int bytesize = 1024 * 1024;

                string message = null;
                byte[] buffer = new byte[bytesize];

                var sender = listener.AcceptTcpClient();
                sender.GetStream().Read(buffer, 0, bytesize);

                // Read the message and perform different actions  
                message = cleanMessage(buffer);

                // Save the data sent by the client;  
                TelemetryDetails telemetry = JsonConvert.DeserializeObject<TelemetryDetails>(message); // Deserialize  

                byte[] bytes = Encoding.Unicode.GetBytes(Thank you for your message);
                sender.GetStream().Write(bytes, 0, bytes.Length); // Send the response  

                await deviceToClude.SendDeviceToCloudMessagesAsync(deviceClient, JsonConvert.SerializeObject(telemetry));
                //sendEmail(person);
            }
        }

        //        private static void sendEmail(Person p)
        //        {
        //            try
        //            {
        //                // Send an email to user also to notify him of the delivery.  
        //                using (SmtpClient client = new SmtpClient(<your-smtp>, 25))
        //                {
        //                    client.EnableSsl = true;
        //                    client.Credentials = new NetworkCredential(<email>, <pass>);

        //                    client.Send(
        //                        new MailMessage(<email-from>, p.Email,
        //                            Thank you for using the Web Service,
        //                            string.Format(
        //    @Thank you for using our Web Service, {0}.   

        //We have recieved your message, '{1}'., p.Name, p.Message
        //                            )
        //                        )
        //                    );
        //                }

        //                Console.WriteLine(Email sent to  + p.Email); // Email sent successfully  
        //            }
        //            catch (Exception e)
        //            {
        //                Console.WriteLine(e.Message);
        //            }
        //        }

        private static string cleanMessage(byte[] bytes)
        {
            string message = Encoding.Unicode.GetString(bytes);

            string messageToPrint = null;
            foreach (var nullChar in message)
            {
                if (nullChar != '\0')
                {
                    messageToPrint += nullChar;
                }
            }
            return messageToPrint;
        }

        // Sends the message string using the bytes provided.  
        //private static void sendMessage(byte[] bytes, TcpClient client)
        //{
        //    client.GetStream()
        //        .Write(bytes, 0,
        //        bytes.Length); // Send the stream  
        //}
    }

    public class DeviceToClude
    {
        //public  async Task Main(string[] args)
        //{
        //    using (var deviceClient = DeviceClient.CreateFromConnectionString(s_deviceConnectionString, s_transportType))
        //    {
        //       // await deviceClient
        //       //.SetMethodHandlerAsync(GetDeviceCommand, ReceiveDeviceCommandAsync, null).ConfigureAwait(false);

        //        await SendDeviceToCloudMessagesAsync(deviceClient);//.ConfigureAwait(false)
        //    }
        //    // Setup a callback for the 'GetDeviceName' method.



        //    Console.WriteLine(Done.\n);
        //    Console.ReadLine();
        //}

        public async Task SendDeviceToCloudMessagesAsync(DeviceClient s_deviceClient, string telemetry)
        {
            try
            {
                //while (true)
                //{
                    //string messageString = GetTelemetryMessages();

                    var message = new Message(Encoding.ASCII.GetBytes(telemetry));
                    await s_deviceClient.SendEventAsync(message);
                   Console.WriteLine(${DateTime.Now} : Telemetry messeage sent. );
                    //await Task.Delay(10000);
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetTelemetryMessages()
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
                    //ApplicationProperties = new ApplicationProperties { MessageType = FoodItemTemperature },
                    //ItemBlobUrl = ,
                    //ItemName = itemName[i]
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

        //private async Task<MethodResponse> ReceiveDeviceCommandAsync(MethodRequest methodRequest, object userContext)
        //{
        //    Console.WriteLine($\t *** {methodRequest.Name} was called.);

        //    MethodResponse retValue;
        //    if (userContext == null)
        //    {
        //        retValue = new MethodResponse(new byte[0], 500);
        //    }
        //    else
        //    {
        //        var data = JsonConvert.DeserializeObject<DirectMethodObject>(Encoding.UTF8.GetString(methodRequest.Data));
        //        string result = JsonConvert.SerializeObject(data);
        //        retValue = new MethodResponse(Encoding.UTF8.GetBytes(result), 200);
        //    }

        //    return Task.FromResult(retValue);
        //}
    }

    

}
