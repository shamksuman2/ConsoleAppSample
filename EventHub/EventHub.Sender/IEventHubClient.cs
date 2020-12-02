namespace EventHub.Sender
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Azure.EventHubs;

    //public interface IEventHubClient : IDisposable
    //{
    //    bool IsClosed { get; }
    //    string PartitionId { get; }
    //    string Path { get; }
    //    RetryPolicy RetryPolicy { get; set; }

    //    void Abort();
    //    Task CloseAsync();
    //}


    public interface IEventHubClient : IDisposable
    {
        RetryPolicy RetryPolicy { get; set; }

        Task CloseAsync();
    }
}
