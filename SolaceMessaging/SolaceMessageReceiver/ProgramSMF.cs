// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using SolaceSystems.Solclient.Messaging;

class TopicSubscriber //: IDisposable
{
    string VPNName { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    const int DefaultReconnectRetries = 3;
    private ISession Session = null;
    private static EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
    
    static void Main(string[] args)
    {
        ContextFactoryProperties cfp = new ContextFactoryProperties()
        {
            SolClientLogLevel = SolLogLevel.Warning
        };
        cfp.LogToConsoleError();
        ContextFactory.Instance.Init(cfp);

        //X509CertificateCollection certificatesCollection = new X509CertificateCollection();
        //foreach (StoreLocation storeLocation in (StoreLocation[])Enum.GetValues(typeof(StoreLocation)))
        //{
        //    foreach (StoreName storeName in (StoreName[])Enum.GetValues(typeof(StoreName)))
        //    {
        //        X509Store store = new X509Store(storeName, storeLocation);
        //        try
        //        {
        //            store.Open(OpenFlags.OpenExistingOnly);
        //            foreach (X509Certificate certificate in store.Certificates)
        //            {
        //                certificatesCollection.Add(certificate);
        //            }
        //        }
        //        catch (CryptographicException)
        //        {
        //            Console.WriteLine("No      {0}, {1}", store.Name, store.Location);
        //        }
        //    }
        //    Console.WriteLine();
        //}

        if (!File.Exists(@"C:\Users\zzu2kor\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessageReceiver\DigiCertGlobalRootCA.crt.pem"))
        { // Create a file to write to}
        }
        SessionProperties sessionProps = new SessionProperties()
        {
            Host = "tcp://mr-connection-l7ua5om65da.messaging.solace.cloud:55443",
            VPNName = "solace1sttryout",
            UserName = "solace-cloud-client",
            ClientName = "shamsolate01",
            Password = "pflp78r0u6ej8q7hntd7l4ncqp",
            ReconnectRetries = DefaultReconnectRetries,
            //SSLTrustStore = certificatesCollection
            SSLValidateCertificate = false
            //SSLTrustStoreDir = @"C:\Users\zzu2kor\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessageReceiver"
        };

        using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
        using (ISession session = context.CreateSession(sessionProps, null, null))
        {
            ReturnCode returnCode = session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                // connected to the Solace message router
                Console.WriteLine("Session successfully connected.");

                session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/topic"), true);

                Console.WriteLine("Waiting for a message to be published...");
                WaitEventWaitHandle.WaitOne();
            }
        }
    }
    private static void HandleMessage(object source, MessageEventArgs args)
    {
        Console.WriteLine("Received published message.");
        // Received a message
        using (IMessage message = args.Message)
        {
            var receivedmessage =
                    $"DateTime {DateTime.Now.ToString()}, data : {JsonSerializer.Serialize(message)}";
            var path = @"C:\Users\ZZU2KOR\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessaging\ReceivedData.txt";
            WriteToFile(path, receivedmessage);

            // Expecting the message content as a binary attachment
            Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
            // finish the program
            WaitEventWaitHandle.Set();
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


}