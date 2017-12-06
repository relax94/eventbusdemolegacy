using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusDemoLegacy
{
    public class Consumer : IEventProcessor
    {
        private Stopwatch checkpointStopwatch;

        Task IEventProcessor.OpenAsync(PartitionContext context)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Listen Partition {context.Lease.PartitionId}, at position {context.Lease.Offset}");
            this.checkpointStopwatch = Stopwatch.StartNew();

            return Task.FromResult<object>(null);
        }

        async Task IEventProcessor.ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (var eventData in messages)
            {
                var data = Encoding.UTF8.GetString(eventData.GetBytes());

                Console.WriteLine($"Message recieved : partition {context.Lease.PartitionId}, Message = {data}");
            }

            if (this.checkpointStopwatch.Elapsed > TimeSpan.FromMinutes(5))
            {
                await context.CheckpointAsync();
                this.checkpointStopwatch.Restart();
            }
        }

        async Task IEventProcessor.CloseAsync(PartitionContext context, CloseReason reason)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Releasing lease of partition {context.Lease.PartitionId} Reason {reason}");
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }
    }
}
