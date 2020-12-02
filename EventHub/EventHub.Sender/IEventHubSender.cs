namespace EventHub.Sender
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Azure.EventHubs;

    //public interface IEventHubSender : IEventHubClient
    //{
    //    Task SendAsync(EventData data);
    //    Task SendBytesAsync(byte[] data);
    //    Task SendBatchAsync(IEnumerable<EventData> data);
    //}

    public interface IEventHubSender : IEventHubClient
    {
        Task SendAsync(EventData data);
        Task SendAsync(EventData data, string partitionId);
        Task SendBatchAsync(IEnumerable<EventData> data);
        Task SendBytesAsync(byte[] data);
    }
}
