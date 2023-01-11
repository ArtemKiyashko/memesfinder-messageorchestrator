using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator
{
    public class MessageOrchestrator
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusMessageSender;

        public MessageOrchestrator(
            ILogger<MessageOrchestrator> log,
            IServiceBusClient serviceBusMessageSender)
        {
            _logger = log;
            _serviceBusMessageSender = serviceBusMessageSender;
        }

        [FunctionName("MessageOrchestrator")]
        public void Run([ServiceBusTrigger("allmessages", "messageorchestrator", Connection = "ServiceBusOptions")] Update tgMessages)
        {
            _logger.LogInformation($"C# ServiceBus topic trigger function processed message: {tgMessages}");
        }
    }
}
