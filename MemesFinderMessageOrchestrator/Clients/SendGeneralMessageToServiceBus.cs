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
    //send object to the server to General topic
    public class SendGeneralMessageToServiceBus : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<SendGeneralMessageToServiceBus> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;

        public SendGeneralMessageToServiceBus(ILogger<SendGeneralMessageToServiceBus> log, IServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> options)
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