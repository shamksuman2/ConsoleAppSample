using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Azure.Devices.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using DeviceRegWithNetFramework.Library;
using Microsoft.Extensions.DependencyInjection;
using ConfigurationBuilder = Microsoft.Extensions.Configuration.ConfigurationBuilder;

namespace DeviceRegWithNetFramework
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        private static ILogger logger;
        private static string deviceId;
        static async Task Main(string[] args)
        {
            System.Console.WriteLine("Hello World!");

            try
            {

                Configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    //.AddEnvironmentVariables()
                    .Build();


                var serviceProvider = new ServiceCollection().AddLogging(cfg => cfg.AddConsole())
                    .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Debug).BuildServiceProvider();
                // get instance of logger
                logger = serviceProvider.GetService<ILogger<Program>>();
                // use the logger
                //logger.LogDebug("Woo Hooo");


                Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();

                Log.Information("Logger is working! from program main");


                var certificatePath = Environment.GetEnvironmentVariable("ANTHEMGSDG_CERTPATH");


                //AppSettings.CertificateStoreOverride = string.IsNullOrEmpty(certificatePath) ? AppSettings.CertificateStoreOverride : certificatePath;
                var certificateStoreOverride = string.IsNullOrEmpty(certificatePath) ? "" : certificatePath;
                var clientCert = CryptoUtils.LoadCertificate("Dover Devices Client Certificate", "");
                if (clientCert == null)
                {
                    //log - factory.GetInstance<ILogger>().Error("Unable to locate a device certificate.");
                }

                var UuIdNumber = 3;

                var deviceRegistor = new DeviceIdGenerator(null, clientCert, PlatformID.Win32NT);
                deviceId = deviceRegistor.GenerateDeviceId(UuIdNumber).ToString();
                var certificate = new X509Certificate2(clientCert);
                var deviceClient = new DeviceClientWrapper("jupiter-qa.azure-devices.net", deviceId, "Terminal",
                    certificate, Log.Logger, false);


                var loggerDeviceRegistrator = serviceProvider.GetService<ILogger<DeviceRegistrator>>();

                var devciDeviceRegistrator = new DeviceRegistrator(Log.Logger, deviceClient, new Func<string, IFlurlRequest>(Target), deviceId);

                await devciDeviceRegistrator.RegisterDevice();

                await PublishTelemetryManifest(deviceClient, CancellationToken.None);
                await SendAlert(deviceClient);

                var message = GetTelemetryMessages();
                for (int i = 0; i < 10; i++)
                {
                    await SendTelemetrySendDeviceToCloud(deviceClient, message);

                }
                System.Console.ReadLine();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
            }
        }

        private static IFlurlRequest Target(string arg)
        {
            var certificate = new X509Certificate2();

            var client = new FlurlClient("https://api.dataservices-qa.doverfs.com:8863/device")
                .Configure(c => c.HttpClientFactory =
                    certificate != null
                        ? new X509ClientFactory(certificate, Log.Logger)
                        : null);

            return client.Request("register", "Terminal", deviceId);
        }

        private static async Task SendTelemetrySendDeviceToCloud(IDeviceClient deviceClient, string messagebody)
        {
            var msg = new Message(Encoding.UTF8.GetBytes(messagebody));
            msg.Properties.Add("message-type", "Fleet");
            msg.Properties.Add("device-type", "Terminal");
            msg.Properties.Add("message-class", "telemetry");
            await deviceClient.SendEventAsync(msg);
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
                    Temperature = string.Format("{0:0.00}", minTemperature + rand.NextDouble() * 10),
                    ItemId = i.ToString(),
                    ApplicationProperties = new ApplicationProperties { MessageType = "FoodItemTemperature" },
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
            //        MessageType = "TransactionDetails"
            //    }
            //}

            return JsonConvert.SerializeObject(itemcollection);
        }

        static async Task PublishTelemetryManifest(IDeviceClient client, CancellationToken cancel)
        {
            try
            {
                logger.LogInformation("Publishing telemetry manifest to IoT Hub.");
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configuration", "manifest.json");
                var content = File.ReadAllText(path);
                var minifyContent = Regex.Replace(content, @"\s", "");
                await client.SendEventAsync(
                    new Message(Encoding.UTF8.GetBytes(minifyContent))
                        .WithProperties(("message-type", "manifest")));
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "Failed to publish telemetry manifest.");
            }
        }

        static async Task SendAlert(IDeviceClient deviceClient)
        {
            var messageBody = new
            {
                value = "ReceiptLowPaper",
                time = "9/1/2020 8:30:00 AM +01:00"
            };

            var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageBody));
            var message = new Message(messageBytes);
            message.Properties.Add("device-type", "Terminal");
            message.Properties.Add("message-type", "ReceiptLowPaper");
            message.Properties.Add("message-class", "alert");
            await deviceClient.SendEventAsync(message);
        }
    }

    public class AppSetting
    {
        public string DeviceServiceUrl { get; set; }
        public string IoTHubHost { get; set; }
        public string LogLevel { get; set; }
        public string FleetServiceEngineLogsFolder { get; set; }
        public string DeviceCertificateCN { get; set; }
        public string CertificateStoreOverride { get; set; }
        public string DFSStorageRootPath { get; set; }
        public string HeartBeatInterval { get; set; }
        public string fleetcrudserviceurl { get; set; }
        public string UuIdNumber { get; set; }
        public string fleetCardServiceUrl { get; set; }

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
        public string TemperatureUnit { get; set; }

        public string Temperature { get; set; }

        public ApplicationProperties ApplicationProperties { get; set; }
    }

    public class ApplicationProperties
    {
        public string MessageType { get; set; }
    }
}
