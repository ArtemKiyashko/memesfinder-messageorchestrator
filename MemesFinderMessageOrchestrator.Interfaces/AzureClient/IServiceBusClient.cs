using Azure.Messaging.ServiceBus;

namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IServiceBusClient
    {
        public ServiceBusSender CreateSender();
    }
}