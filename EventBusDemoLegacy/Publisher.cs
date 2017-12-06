using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBusDemoLegacy
{
  public class Publisher
    {
        private EventHubClient eventHubClient;

        public void Init(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString));
            }
            eventHubClient = EventHubClient.CreateFromConnectionString(connectionString);
        }

        public void Publish<T>(T eventToPublish) {
            if(eventToPublish == null)
            {
                throw new ArgumentNullException(nameof(eventToPublish));
            }

            var serializedEvent = JsonConvert.SerializeObject(eventToPublish);

            var eventBytes = Encoding.UTF8.GetBytes(serializedEvent);

            var eventData = new EventData(eventBytes);

            this.eventHubClient.Send(eventData);
        }

    }
}
