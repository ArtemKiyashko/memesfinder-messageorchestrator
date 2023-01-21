using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send object to the server if it contains a key word
    public class SendGeneralMessageToServiceBus : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public SendGeneralMessageToServiceBus(ILogger<MessageOrchestrator> log, IServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = options.Value;
        }

        public override async Task SendMessageAsync(Update message)
        {
            await using ServiceBusSender sender = _serviceBusClient.CreateSender(_serviceBusOptions.GeneralMessagesTopic);
            ServiceBusMessage serviceBusMessage = new(message.ToJson());
            await sender.SendMessageAsync(serviceBusMessage);
        }

    }


}