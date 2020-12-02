using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace EventHub.Receiver
{
    [ExcludeFromCodeCoverage]
    public class EventHubReceiver
        : IEventHubReceiver
    {
        PartitionReceiver _receiver;

        public EventHubReceiver(PartitionReceiver receiver)
        {
            _receiver = receiver;
        }

        Task IEventHubReceiver.CloseAsync() => _receiver.CloseAsync();

        Task<IEnumerable<EventData>> IEventHubReceiver.ReceiveAsync(int maxMessageCount, TimeSpan waitTime) => _receiver.ReceiveAsync(maxMessageCount, waitTime);
    }
}
