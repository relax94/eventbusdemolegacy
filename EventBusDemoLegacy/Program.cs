using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusDemoLegacy
{
    class Program
    {

        static void SetupAndStartEventHub()
        {
            const string eventHubConnectionString = "";
            const string eventHubName = "";
            const string storageAccountName = "";
            const string storageAccountKey = "";
            const string storageConnectionString = "";

            var publisher = new Publisher();

            // Init

            var data = new List<string> { "hello", "world" };
            publisher.Publish(data);
            var eventProcessorName = Guid.NewGuid().ToString();
            var eventProcessorHost = new EventProcessorHost(eventProcessorName, eventHubName, EventHubConsumerGroup.DefaultGroupName, eventHubConnectionString, storageConnectionString);
            Console.WriteLine("Registering EventProcessor ... ");
            var options = new EventProcessorOptions();
            options.ExceptionReceived += (sender, e) => { Console.WriteLine(e.Exception); };

            eventProcessorHost.RegisterEventProcessorAsync<Consumer>(options).Wait();

            Console.WriteLine("Receiving ");
            Console.ReadLine();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();
        }

        static void Main(string[] args)
        {
        }
    }
}
