using System.Text;

namespace EventHub.Sender
{
    using Microsoft.Azure.EventHubs;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    //using System.Collections.Generic;
    //using System.Threading.Tasks;
    //using Microsoft.ServiceBus;
    //using Microsoft.ServiceBus.Messaging;

    //[System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    //public class EventHubSenderWrapper : IEventHubSender
    //{
    //    readonly string _connectionString;
    //    readonly object _connectLock = new object();

    //    EventHubSender _sender;
    //    bool _shouldConnect = true;

    //    public EventHubSenderWrapper(string connectionString, string publisher = null)
    //    {
    //        var connetionStringBuilder = new ServiceBusConnectionStringBuilder(connectionString);

    //        if (!string.IsNullOrEmpty(publisher))
    //            connetionStringBuilder.Publisher = publisher;

    //        _connectionString = connetionStringBuilder.ToString();
    //        Connect();
    //    }

    //    private EventHubSender Connect()
    //    {
    //        var sender = _sender;

    //        if (sender != null && !sender.IsClosed)
    //            return sender;

    //        lock (_connectLock)
    //        {
    //            if (!_shouldConnect || (_sender != null && !_sender.IsClosed))
    //                return _sender;

    //            _sender = EventHubSender.CreateFromConnectionString(_connectionString);
    //            return _sender;
    //        }
    //    }

    //    public bool IsClosed => _sender == null || _sender.IsClosed;

    //    public string PartitionId => _sender.PartitionId;

    //    public string Path => _sender.Path;

    //    public RetryPolicy RetryPolicy
    //    {
    //        get { return _sender.RetryPolicy; }
    //        set { _sender.RetryPolicy = value; }
    //    }

    //    public void Abort() => _sender.Abort();

    //    public async Task CloseAsync()
    //    {
    //        _shouldConnect = false;
    //        await _sender.CloseAsync();
    //    }

    //    public Task SendAsync(EventData data) => Connect().SendAsync(data);

    //    public Task SendBytesAsync(byte[] data) => Connect().SendAsync(new EventData(data));

    //    public Task SendBatchAsync(IEnumerable<EventData> data) => Connect().SendBatchAsync(data);

    //    public void Dispose()
    //    {
    //        _shouldConnect = false;
    //        _sender.Close();
    //    }
    //}

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class EventHubSenderWrapper : IEventHubSender
    {
        private readonly EventHubClient _sender;
        private const string EventHubConnectionString = "Endpoint=sb://omtesteventhub.servicebus.windows.net/;SharedAccessKeyName=mypolicy;SharedAccessKey=SIcEW9bMr//4JQxSABrtW/LwoNnE+dTLbc71JI/5S+0=;";
        private const string EventHubName = "omtesteventhub";
        private const string EventHubConsumerGroupName = "$Default";

        public EventHubSenderWrapper()
        {
            EventHubConfig.EventHubName = EventHubName;
            EventHubConfig.WriteConnectionString = EventHubConnectionString;

            var eventHubConfigProvider = EventHubConfig.EventHubConfigProvider;

            var connectionStringBuilder = new EventHubsConnectionStringBuilder(EventHubConfig.WriteConnectionString)
            {
                EntityPath = EventHubConfig.EventHubName
            };

            _sender = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());
        }

        public RetryPolicy RetryPolicy
        {
            get => _sender.RetryPolicy;
            set => _sender.RetryPolicy = value;
        }

        public async Task CloseAsync()
        {
            await _sender.CloseAsync();
        }

        public void Dispose()
        {
            _sender.Close();
        }

        public Task SendAsync(EventData data) => _sender.SendAsync(data);

        public Task SendAsync(EventData data, string partitionId)
        {
            EventData data1 = new EventData(Encoding.UTF8.GetBytes("hello from shambhu"));
            var partitionSender = _sender.CreatePartitionSender(partitionId);
            return partitionSender.SendAsync(data1);
        }

        public Task SendBatchAsync(IEnumerable<EventData> data) => _sender.SendAsync(data);

        public Task SendBytesAsync(byte[] data) => _sender.SendAsync(new EventData(data));
    }
}
