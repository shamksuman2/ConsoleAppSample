namespace CoreReceiverApp
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.ServiceBus;

    class Program
    {
        const string ServiceBusConnectionString = "Endpoint=sb://dataservices-sb-qa.servicebus.windows.net/;SharedAccessKeyName=FleetListener;SharedAccessKey=UTjgH5RLLdkTumszITz5dTdudvnJSrhsPHh4W7L+0ew=;";
        const string SubscriptionName = "fleet";
        //const string SubscriptionName = "fleet";
        //const string SubscriptionName = "fleet-qa2";

        const string TopicName = "statechanges";
        
        static ISubscriptionClient subscriptionClient;

        public static async Task Main(string[] args)
        {
            subscriptionClient = new SubscriptionClient(ServiceBusConnectionString, TopicName, SubscriptionName);

            Console.WriteLine("=======================site onboarding , statechanges =================");
            Console.WriteLine("Press ENTER key to exit after receiving all the messages.");
            Console.WriteLine("======================================================");

            // Register subscription message handler and receive messages in a loop
            RegisterOnMessageHandlerAndReceiveMessages();
            //purge messages
            //PurgeMessagesFromSubscription();
            Console.ReadKey();

            await subscriptionClient.CloseAsync();
        }

        static void RegisterOnMessageHandlerAndReceiveMessages()
        {
            // Configure the message handler options in terms of exception handling, number of concurrent messages to deliver, etc.
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                // Maximum number of concurrent calls to the callback ProcessMessagesAsync(), set to 1 for simplicity.
                // Set it according to how many messages the application wants to process in parallel.
                MaxConcurrentCalls = 1,

                // Indicates whether MessagePump should automatically complete the messages after returning from User Callback.
                // False below indicates the Complete will be handled by the User Callback as in `ProcessMessagesAsync` below.
                AutoComplete = false
            };

            // Register the function that processes messages.
            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        static async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            // Process the message.

            if (message.UserProperties["IndexName"].ToString() == "site")
           {
                var receivedmessage = $"Received message: SequenceNumber:{message.MessageId} Body:{Encoding.UTF8.GetString(message.Body)}";
                var path = @"D:\Assignment\ConsoleApp\ServiceBus\ServiceBusReceiver1\siteonboarding.txt";
                WriteToFile(path, receivedmessage);
            }
            // Complete the message so that it is not received again.
            // This can be done only if the subscriptionClient is created in ReceiveMode.PeekLock mode (which is the default).
            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);

            // Note: Use the cancellationToken passed as necessary to determine if the subscriptionClient has already been closed.
            // If subscriptionClient has already been closed, you can choose to not call CompleteAsync() or AbandonAsync() etc.
            // to avoid unnecessary exceptions.
        }

        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            Console.WriteLine($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            Console.WriteLine("Exception context for troubleshooting:");
            Console.WriteLine($"- Endpoint: {context.Endpoint}");
            Console.WriteLine($"- Entity Path: {context.EntityPath}");
            Console.WriteLine($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }


        static void PurgeMessagesFromSubscription()
        {
            int batchSize = 100;
            do
            {
                //var messages = subscriptionClient.ReceiveBatch(batchSize);
                //if (messages.Count() == 0)
                //{
                //    break;
                //}
            }
            while (true);
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
}