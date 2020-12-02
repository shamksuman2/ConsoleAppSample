using Microsoft.Azure.EventHubs;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Fabric;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EventHub.Receiver
{
    [ExcludeFromCodeCoverage]
    public class EventHubReceiverFactory
        : IEventHubReceiverFactory
    {
        private readonly string _connectionString;
        private readonly string _consumerGroupName;
        private readonly DateTime EpochStart = DateTime.UtcNow.AddDays(-1);    //  delta is secondes stored in 32bit int gives 68 years until wraping 

        public EventHubReceiverFactory(string connectionString, string consumerGroup)
        {
            _connectionString = connectionString;
            _consumerGroupName = consumerGroup;
        }

        async Task<IEventHubReceiver> IEventHubReceiverFactory.CreateReceiverAsync(EventPosition startPosition, StatefulServiceContext context, CancellationToken cancellationToken)
        {
            var eventHubClient = EventHubClient.CreateFromConnectionString(_connectionString);
            var hubPartition = await MapServicePartitionToEventHubPartitionAsync(eventHubClient, context, cancellationToken);

            //  ever increasing epoch, new service instances always close old service instances
            var epoch = (int)(DateTime.UtcNow - EpochStart).TotalSeconds;

            var receiver = eventHubClient.CreateEpochReceiver(_consumerGroupName, hubPartition, startPosition, epoch);
            return new EventHubReceiver(receiver);
        }

        private async Task<string> MapServicePartitionToEventHubPartitionAsync(EventHubClient eventHubClient, StatefulServiceContext context, CancellationToken cancellationToken)
        {
            var runtimeInfo = await eventHubClient.GetRuntimeInformationAsync();


            using (var client = new FabricClient())
            {
                var partitions = await client.QueryManager.GetPartitionListAsync(context.ServiceName);
               
                if (partitions.Count() != runtimeInfo.PartitionCount)
                {
                    throw new InvalidOperationException("The number of event hub partitions and stateful partition has to match.");
                }


                var int64PartitionInfo = partitions.Select(x => x.PartitionInformation as Int64RangePartitionInformation).FirstOrDefault(l => l.Id == context.PartitionId);
                if (int64PartitionInfo != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Id: {int64PartitionInfo.Id}, Low: {int64PartitionInfo.LowKey} - {int64PartitionInfo.HighKey}");
                    return int64PartitionInfo.LowKey.ToString();
                    
                }
                throw new InvalidOperationException($"Could not find Partition Key for PartitionId: {context.PartitionId}");
            }
        }
    }
}
