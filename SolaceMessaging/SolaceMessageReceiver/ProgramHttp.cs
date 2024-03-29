﻿// See https://aka.ms/new-console-template for more information
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using SolaceSystems.Solclient.Messaging;

class TopicSubscriber : IDisposable
{
    string VPNName { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    const int DefaultReconnectRetries = 3;
    private ISession Session = null;
    private static EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
    #region main
    static void Main(string[] args)
    {
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
                using (TopicSubscriber topicSubscriber = new TopicSubscriber()
                {
                    VPNName = "solace1sttryout",
                    UserName = "solace-cloud-client",
                    Password = "2ab9ftihbqtc62l3gt3uaes8i3"
            })
                {
                    //"wss://mr-connection-l7ua5om65da.messaging.solace.cloud:443"
                    // Run the application within the context and against the host
                    topicSubscriber.Run(context, "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception thrown: {0}", ex.Message);
        }
        finally
        {
            // Dispose Solace Systems Messaging API
            ContextFactory.Instance.Cleanup();
        }
        Console.WriteLine("Finished.");
    }

    void Run(IContext context, string host)
    {
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
        Console.WriteLine("Connecting as {0}@{1} on {2}...", UserName, VPNName, host);
        // NOTICE HandleRequestMessage as the message event handler
        Session = context.CreateSession(sessionProps, HandleRequestMessage, null);
        ReturnCode returnCode = Session.Connect();
        if (returnCode == ReturnCode.SOLCLIENT_OK)
        {
            Console.WriteLine("Session successfully connected.");

            // This is the topic on Solace messaging router where a request is placed
            // The reply must subscribe to it to receive requests
            Session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/requests1"), true);

            Console.WriteLine("Waiting for a request to come in...");
            WaitEventWaitHandle.WaitOne();
        }
        else
        {
            Console.WriteLine("Error connecting, return code: {0}", returnCode);
        }
    }

    /// <summary>
    /// This event handler is invoked by Solace Systems Messaging API when a message arrives
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    private void HandleRequestMessage(object source, MessageEventArgs args)
    {
        Console.WriteLine("Received request.");
        // Received a request message
        using (IMessage requestMessage = args.Message)
        {
            Console.WriteLine("Received published message.");
            // Received a message
            using (IMessage message = args.Message)
            {
                // Expecting the message content as a binary attachment
                var msg = Encoding.ASCII.GetString(message.BinaryAttachment);
                Console.WriteLine("Message content: {0}", msg);
                var receivedmessage =
                                $"DateTime {DateTime.Now.ToString()}, data : {msg}";
                var path = @"C:\Users\ZZU2KOR\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessaging\ReceivedData.txt";
                WriteToFile(path, receivedmessage);


                // finish the program
                WaitEventWaitHandle.Set();
            }
        }
    }

    #region IDisposable Support
    private bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                if (Session != null)
                {
                    Session.Dispose();
                }
            }
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion

    #endregion
    //static void Main(string[] args)
    //{
    //    ContextFactoryProperties cfp = new ContextFactoryProperties()
    //    {
    //        SolClientLogLevel = SolLogLevel.Warning
    //    };
    //    cfp.LogToConsoleError();
    //    ContextFactory.Instance.Init(cfp);

    //    SessionProperties sessionProps = new SessionProperties()
    //    {
    //        Host = "tcps://mr-connection-l7ua5om65da.messaging.solace.cloud:55443",
    //        VPNName = "solace1sttryout",
    //        UserName = "solace-cloud-client",
    //        Password = "pflp78r0u6ej8q7hntd7l4ncqp",
    //        ReconnectRetries = DefaultReconnectRetries
    //    };

    //    using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
    //    using (ISession session = context.CreateSession(sessionProps, null, null))
    //    {
    //        ReturnCode returnCode = session.Connect();
    //        if (returnCode == ReturnCode.SOLCLIENT_OK)
    //        {
    //            // connected to the Solace message router
    //            Console.WriteLine("Session successfully connected.");

    //            session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/topic"), true);

    //            Console.WriteLine("Waiting for a message to be published...");
    //            WaitEventWaitHandle.WaitOne();
    //        }
    //    }
    //}
    //private static void HandleMessage(object source, MessageEventArgs args)
    //{
    //    Console.WriteLine("Received published message.");
    //    // Received a message
    //    using (IMessage message = args.Message)
    //    {
    //        var receivedmessage =
    //                $"DateTime {DateTime.Now.ToString()}, data : {JsonSerializer.Serialize(message)}";
    //        var path = @"C:\Users\ZZU2KOR\OneDrive - Bosch Group\tryouts\git source\ConsoleAppSample\SolaceMessaging\SolaceMessaging\ReceivedData.txt";
    //        WriteToFile(path, receivedmessage);

    //        // Expecting the message content as a binary attachment
    //        Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
    //        // finish the program
    //        WaitEventWaitHandle.Set();
    //    }
    //}
    private static void WriteToFile(string path, string data)
    {
        using (var fs = new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
        using (var sw = new StreamWriter(fs))
        {
            sw.WriteLine(data + Environment.NewLine + "-------------------------------------------------------------" + Environment.NewLine);
        }
    }


}