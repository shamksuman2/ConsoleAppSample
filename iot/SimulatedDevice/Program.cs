    using Microsoft.Azure.Devices;
using Polly;
using System;
using System.Text;
using System.Threading.Tasks;

namespace SimulatedDevice
{
    class Program
    {
        private static ServiceClient serviceClient;
        private static int retryinterval=1;
        private static string deviceid = "00397cc7-520e-d476-c0ee-5b3d9171a841";
        private static string iotconn = "HostName=Jupiter-QA.azure-devices.net;SharedAccessKeyName=service;SharedAccessKey=qA4TkddEUOciRu06y6qSgOwmYF1pXuXHstssuhGXORQ=";

        static async Task Main(string[] args)
        {
            try
            {
                await RunSample();

            }
            catch (Exception ex)
            {
            }
            //ReceiveC2dAsync();
            Console.ReadLine();

        }

        private static async Task RunSample()
        {
            var retry = Policy
            .Handle<Exception>()
            .WaitAndRetryForeverAsync(sleepDurationProvider => TimeSpan.FromSeconds(10));

            //.WaitAndRetryAsync(retryinterval, retryAttempt => TimeSpan.FromSeconds(30));

               await retry.ExecuteAsync( async () =>
                {
                    retryinterval += 1;
                    await InvokeMethod();

                });

        }

        private static async Task InvokeMethod()
        {

            serviceClient = ServiceClient.CreateFromConnectionString(iotconn, TransportType.Amqp);

            var methodInvocation = new CloudToDeviceMethod("SetTelemetryInterval") { ResponseTimeout = TimeSpan.FromSeconds(30) };
            methodInvocation.SetPayloadJson("10");

            await serviceClient.InvokeDeviceMethodAsync(deviceid, methodInvocation);


            //Console.WriteLine(Response status: {0}, payload - sham: , response.Status);
            //Console.WriteLine(response.GetPayloadAsJson());
        }
        private static async void ReceiveC2dAsync()
        {
            Console.WriteLine("\nReceiving cloud to device messages from service");
            while (true)
            {
                // Message receivedMessage = await _deviceClient.ReceiveAsync();
                //if (receivedMessage == null) continue;

                Console.ForegroundColor = ConsoleColor.Yellow;
                //Console.WriteLine(Received message: {0},
                //Encoding.ASCII.GetString(receivedMessage.GetBytes()));
                //Console.ResetColor();

                //await _deviceClient.CompleteAsync(receivedMessage);
            }
        }
    }
}
