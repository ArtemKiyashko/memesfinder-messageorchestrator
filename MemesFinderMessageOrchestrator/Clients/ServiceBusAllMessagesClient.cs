using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Options;

namespace MemesFinderMessageOrchestrator.Clients
{
    public class ServiceBusAllMessagesClient : IServiceBusClient
    {
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public ServiceBusAllMessagesClient(ServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> serviceBusOptions)
        {
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = serviceBusOptions.Value;
        }


        public ServiceBusSender CreateSender()
        {
            return _serviceBusClient.CreateSender("allmessages");
        }
    }
}
