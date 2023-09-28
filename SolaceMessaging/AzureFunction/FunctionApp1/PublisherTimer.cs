using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SolaceSystems.Solclient.Messaging;

namespace FunctionApp1
{
    public static class PublisherTimer
    {
        //private static readonly ILogger _logger;
        static string VPNName { get; set; }
        static string UserName { get; set; }
        static string Password { get; set; }

        const int DefaultReconnectRetries = 3;
        //public PublisherTimer(ILoggerFactory loggerFactory)
        //{
        //    _logger = loggerFactory.CreateLogger<PublisherTimer>();
        //}

        [Function("PublisherTimer")]
        public static void Run1111([TimerTrigger("0 * * * * *")] MyInfo myTimer, ILogger _logger)
        {
            _logger.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");

            string proxy = "httpc://rb-proxy-de.bosch.com:8080";
            string host = $"tcp://mr-connection-l7ua5om65da.messaging.solace.cloud:55555%{proxy}";
            string username = "solace-cloud-client";
            string vpnname = "solace1sttryout";
            string password = "pflp78r0u6ej8q7hntd7l4ncqp";

            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);

            try
            {
                // Context must be created first
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    // Create the application
                    VPNName = vpnname;
                    UserName = username;
                    Password = password;

                    // Run the application within the context and against the host
                    Run1(context, host, _logger);

                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }
            _logger.LogInformation("Finished.");

            _logger.LogInformation($"Next timer schedule at: {myTimer.ScheduleStatus.Next}");
        }
        static void Run1(IContext context, string host, ILogger _logger)
        {
            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = host,
                VPNName = VPNName,
                UserName = UserName,
                Password = Password,
                ReconnectRetries = DefaultReconnectRetries,
                SSLValidateCertificate = false,
                //SSLClientCertificateFile = sslFile
            };


            // Connect to the Solace messaging router
            _logger.LogInformation("Connecting as {0}@{1} on {2}...", UserName, VPNName, host);
            using (ISession session = context.CreateSession(sessionProps, null, null))
            {
                ReturnCode returnCode = session.Connect();
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    _logger.LogInformation("Session successfully connected.");
                }
                else
                {
                    _logger.LogInformation("Error connecting, return code: {0}", returnCode);
                }
            }
        }
    
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
