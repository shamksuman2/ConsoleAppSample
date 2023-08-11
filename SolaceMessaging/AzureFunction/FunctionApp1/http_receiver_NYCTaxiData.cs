using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using SolaceSystems.Solclient.Messaging;
using System.Threading;
using ISession = SolaceSystems.Solclient.Messaging.ISession;
using System.Text;
using System.IO;

namespace ReceiveNYCTaxiDataNS
{

    public static class http_receiver_NYCTaxiData
    {

        static string VPNName { get; set; }
        static string UserName { get; set; }
        static string Password { get; set; }
        const int DefaultReconnectRetries = 3;
        private static ISession Session = null;
        private static EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
        static ILogger  log = null;
        [FunctionName("ReceiveNYCTaxiDataFunc")]
        public static IActionResult http_receiver_NYCTaxiData_fn(
               [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, ILogger log1)

        {
            log1 = log1 ?? throw new ArgumentNullException(nameof(log1));

            log.LogInformation("http trigger function processed a request.");

            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);
            try
            {
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    //VPNName = "solace1sttryout";
                    //UserName = "solace-cloud-client";
                    //Password = "pflp78r0u6ej8q7hntd7l4ncqp";
                    //Run(context, "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443");

                    //VPNName = "nyc-modern-taxi";
                    //UserName = "public-taxi-user";
                    //Password = "iliketaxis";
                    //Run(context, "https://taxi.messaging.solace.cloud:9443");

                    VPNName = "emb-aws-d-v020-dessoldev";
                    UserName = "solace-cloud-client";
                    Password = "21atvu2kvfqt83okvnqhp2r53d";
                    Run(context, "https://mr-connection-8cbir63661i.messaging.solace.cloud:9443");
                }
            }
            catch (Exception ex)
            {
                log.LogInformation("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }
            log.LogInformation("Finished.");
            return new OkObjectResult(null);
        }

        static void Run(IContext context, string host)
        {
            log.LogInformation("Starting Run method...");
            // Validate parameters
            if (context == null)
            {
                throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
            }
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Solace Messaging Router host name must be non-empty.", "host");
            }
            if (string.IsNullOrWhiteSpace(VPNName))
            {
                throw new InvalidOperationException("VPN name must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new InvalidOperationException("Client username must be non-empty.");
            }
            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = host,
                VPNName = VPNName,
                UserName = UserName,
                Password = Password,
                ReconnectRetries = DefaultReconnectRetries
            };

            // Connect to the Solace messaging router
            log.LogInformation($"Starting Run method with Host : {host}, VPN:{VPNName}, UserName : {UserName}, Password: {Password}");
            // NOTICE HandleRequestMessage as the message event handler
            Session = context.CreateSession(sessionProps, HandleRequestMessage, null);
            log.LogInformation("Connecting as Sesson");

            ReturnCode returnCode = Session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                log.LogInformation("Session successfully connected.");

                // This is the topic on Solace messaging router where a request is placed
                // The reply must subscribe to it to receive requests
                Session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/requests1"), true);

                log.LogInformation("Waiting for a request to come in...");
                WaitEventWaitHandle.WaitOne();
            }
            else
            {
                log.LogInformation("Error connecting, return code: {0}", returnCode);
            }


        }
        private static void HandleRequestMessage(object source, MessageEventArgs args)
        {
            log.LogInformation("Received request.");
            // Received a request message
            using (IMessage requestMessage = args.Message)
            {
                log.LogInformation("Received published message.");
                // Received a message
                using (IMessage message = args.Message)
                {
                    // Expecting the message content as a binary attachment
                    var msg = Encoding.ASCII.GetString(message.BinaryAttachment);
                    log.LogInformation("Message content: {0}", msg);
                    var receivedmessage =
                                    $"DateTime {DateTime.Now.ToString()}, data : {msg}";
                    var path = @"C:\Users\ZZU2KOR\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessaging\ReceivedData.txt";
                    WriteToFile(path, receivedmessage);


                    // finish the program
                    WaitEventWaitHandle.Set();
                }
            }
        }

        private static void WriteToFile(string path, string data)
        {
            using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine(data + Environment.NewLine + "-------------------------------------------------------------" + Environment.NewLine);
            }
        }
        //public class TopicSubscriber : IDisposable
        //{
        //    private bool disposedValue;

        //    public string VPNName { get; set; }
        //    public string UserName { get; set; }
        //    public string Password { get; set; }

        //    protected virtual void Dispose(bool disposing)
        //    {
        //        if (!disposedValue)
        //        {
        //            if (disposing)
        //            {
        //                // TODO: dispose managed state (managed objects)
        //            }

        //            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
        //            // TODO: set large fields to null
        //            disposedValue = true;
        //        }
        //    }

        //    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        //    // ~TopicSubscriber()
        //    // {
        //    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //    //     Dispose(disposing: false);
        //    // }

        //    public void Dispose()
        //    {
        //        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //        Dispose(disposing: true);
        //        GC.SuppressFinalize(this);
        //    }
        //}
    }


}
