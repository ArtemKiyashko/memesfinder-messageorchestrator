using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send object to the server 
    public class SendMessageToServiceBus : ISendMessageToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;

        public SendMessageToServiceBus(ILogger<MessageOrchestrator> log, IServiceBusClient serviceBusClient)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
        }

        public async Task SendMessageAsync(Update message)
        {
            await using ServiceBusSender sender = _serviceBusClient.CreateSender();
            ServiceBusMessage serviceBusMessage = new(message.ToJson());
            await sender.SendMessageAsync(serviceBusMessage);
        }

    }


}