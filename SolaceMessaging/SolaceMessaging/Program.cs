// See https://aka.ms/new-console-template for more information
using System.Text;
using SolaceSystems.Solclient.Messaging;

class BasicReplier //: IDisposable
{
    string VPNName { get; set; }
    string UserName { get; set; }
    string Password { get; set; }
    const int DefaultReconnectRetries = 3;
    private ISession Session = null;
    private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);
    #region main
    //static void Main1(string[] args)
    //{
    //    ContextFactoryProperties cfp = new ContextFactoryProperties()
    //    {
    //        SolClientLogLevel = SolLogLevel.Warning
    //    };
    //    cfp.LogToConsoleError();
    //    ContextFactory.Instance.Init(cfp);
    //    try
    //    {
    //        using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
    //        {
    //            using (BasicReplier basicReplier = new BasicReplier()
    //            {
    //                VPNName = "solace1sttryout",
    //                UserName = "solace-cloud-client",
    //                Password = "pflp78r0u6ej8q7hntd7l4ncqp"
    //            })
    //            {
    //                //"wss://mr-connection-l7ua5om65da.messaging.solace.cloud:443"
    //                // Run the application within the context and against the host
    //                basicReplier.Run(context, "ssl://mr-connection-l7ua5om65da.messaging.solace.cloud:8883");
    //            }
    //        }
    //    }
    //    //using (ISession session = context.CreateSession(sessionProps, HandleMessage, null))
    //    //{
    //    //    ReturnCode returnCode = session.Connect();
    //    //    if (returnCode == ReturnCode.SOLCLIENT_OK)
    //    //    {
    //    //        // connected to the Solace message router
    //    //    }
    //    //}
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine("Exception thrown: {0}", ex.Message);
    //    }
    //    finally
    //    {
    //        // Dispose Solace Systems Messaging API
    //        ContextFactory.Instance.Cleanup();
    //    }
    //    Console.WriteLine("Finished.");
    //}

    //void Run(IContext context, string host)
    //{
    //    // Validate parameters
    //    if (context == null)
    //    {
    //        throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
    //    }
    //    if (string.IsNullOrWhiteSpace(host))
    //    {
    //        throw new ArgumentException("Solace Messaging Router host name must be non-empty.", "host");
    //    }
    //    if (string.IsNullOrWhiteSpace(VPNName))
    //    {
    //        throw new InvalidOperationException("VPN name must be non-empty.");
    //    }
    //    if (string.IsNullOrWhiteSpace(UserName))
    //    {
    //        throw new InvalidOperationException("Client username must be non-empty.");
    //    }

    //    // Create session properties
    //    SessionProperties sessionProps = new SessionProperties()
    //    {
    //        Host = host,
    //        VPNName = VPNName,
    //        UserName = UserName,
    //        Password = Password,
    //        ReconnectRetries = DefaultReconnectRetries
    //    };

    //    // Connect to the Solace messaging router
    //    Console.WriteLine("Connecting as {0}@{1} on {2}...", UserName, VPNName, host);
    //    // NOTICE HandleRequestMessage as the message event handler
    //    Session = context.CreateSession(sessionProps, HandleRequestMessage, null);
    //    ReturnCode returnCode = Session.Connect();
    //    if (returnCode == ReturnCode.SOLCLIENT_OK)
    //    {
    //        Console.WriteLine("Session successfully connected.");

    //        // This is the topic on Solace messaging router where a request is placed
    //        // The reply must subscribe to it to receive requests
    //        Session.Subscribe(ContextFactory.Instance.CreateTopic("tutorial/requests1"), true);

    //        Console.WriteLine("Waiting for a request to come in...");
    //        WaitEventWaitHandle.WaitOne();
    //    }
    //    else
    //    {
    //        Console.WriteLine("Error connecting, return code: {0}", returnCode);
    //    }
    //}

    ///// <summary>
    ///// This event handler is invoked by Solace Systems Messaging API when a message arrives
    ///// </summary>
    ///// <param name="source"></param>
    ///// <param name="args"></param>
    //private void HandleRequestMessage(object source, MessageEventArgs args)
    //{
    //    Console.WriteLine("Received request.");
    //    // Received a request message
    //    using (IMessage requestMessage = args.Message)
    //    {
    //        // Expecting the request content as a binary attachment
    //        Console.WriteLine("Request content: {0}", Encoding.ASCII.GetString(requestMessage.BinaryAttachment));
    //        // Create reply message
    //        using (IMessage replyMessage = ContextFactory.Instance.CreateMessage())
    //        {
    //            // Set the reply content as a binary attachment 
    //            replyMessage.BinaryAttachment = Encoding.ASCII.GetBytes("Sample Reply");
    //            Console.WriteLine("Sending reply...");
    //            ReturnCode returnCode = Session.SendReply(requestMessage, replyMessage);
    //            if (returnCode == ReturnCode.SOLCLIENT_OK)
    //            {
    //                Console.WriteLine("Sent.");
    //            }
    //            else
    //            {
    //                Console.WriteLine("Reply failed, return code: {0}", returnCode);
    //            }
    //            // finish the program
    //            WaitEventWaitHandle.Set();
    //        }
    //    }
    //}

    //#region IDisposable Support
    //private bool disposedValue = false;

    //protected virtual void Dispose(bool disposing)
    //{
    //    if (!disposedValue)
    //    {
    //        if (disposing)
    //        {
    //            if (Session != null)
    //            {
    //                Session.Dispose();
    //            }
    //        }
    //        disposedValue = true;
    //    }
    //}

    //public void Dispose()
    //{
    //    Dispose(true);
    //}
    //#endregion

#endregion
    static void Main(string[] args)
    {
        ContextFactoryProperties cfp = new ContextFactoryProperties()
        {
            SolClientLogLevel = SolLogLevel.Warning
        };
        cfp.LogToConsoleError();
        ContextFactory.Instance.Init(cfp);

        SessionProperties sessionProps = new SessionProperties()
        {
            Host = "https://mr-connection-l7ua5om65da.messaging.solace.cloud:9443",
            VPNName = "default",
            UserName = "solace-cloud-client",
            Password = "pflp78r0u6ej8q7hntd7l4ncqp",
            ReconnectRetries = DefaultReconnectRetries
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
                //WaitEventWaitHandle.WaitOne();
            }
        }
    }
    private static void HandleMessage(object source, MessageEventArgs args)
    {
        Console.WriteLine("Received published message.");
        // Received a message
        using (IMessage message = args.Message)
        {
            // Expecting the message content as a binary attachment
            Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
            // finish the program
            //WaitEventWaitHandle.Set();
        }
    }
}