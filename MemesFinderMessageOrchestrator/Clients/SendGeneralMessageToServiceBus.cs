using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send object to the server if it contains a key word
    public class SendGeneralMessageToServiceBus : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly string _topic = "generalmessages";

        public SendGeneralMessageToServiceBus(ILogger<MessageOrchestrator> log, IServiceBusClient serviceBusClient)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
        }

        public override async Task SendMessageAsync(Update message)
        {
            await using ServiceBusSender sender = _serviceBusClient.CreateSender(_topic);
            ServiceBusMessage serviceBusMessage = new(message.ToJson());
            await sender.SendMessageAsync(serviceBusMessage);
        }

    }


}