using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventHub.Receiver
{
    public interface IEventHubReceiver
    {
        Task CloseAsync();
        Task<IEnumerable<EventData>> ReceiveAsync(int maxMessageCount, TimeSpan waitTime);
    }
}
