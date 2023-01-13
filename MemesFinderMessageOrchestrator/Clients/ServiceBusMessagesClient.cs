using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Options;

namespace MemesFinderMessageOrchestrator.Clients
{
    public class ServiceBusMessagesClient : IServiceBusClient
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusMessagesClient(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> serviceBusOptions)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = serviceBusOptions.Value;
        }


        public ServiceBusSender CreateSender(string topic)
        {
            return _serviceBusClient.CreateSender(topic);
        }
    }
}
