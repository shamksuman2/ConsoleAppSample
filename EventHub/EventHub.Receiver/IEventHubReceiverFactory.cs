using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHub.Receiver
{
    public interface IEventHubReceiverFactory
    {
        Task<IEventHubReceiver> CreateReceiverAsync(EventPosition startPosition, StatefulServiceContext context, CancellationToken cancellationToken);
    }
}
