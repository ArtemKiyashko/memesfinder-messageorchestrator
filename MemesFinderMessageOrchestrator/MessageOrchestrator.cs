using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator
{
    public class MessageOrchestrator
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusMessageSender;
        private readonly IValidator<Message> _messageValidator;

        public MessageOrchestrator(
            ILogger<MessageOrchestrator> log,
            IServiceBusClient serviceBusMessageSender,
            IValidator<Message> messageValidator)
        {
            _logger = log;
            _serviceBusMessageSender = serviceBusMessageSender;
            _messageValidator = messageValidator;
        }

        [FunctionName("MessageOrchestrator")]
        public async Task Run([ServiceBusTrigger("allmessages", "messageorchestrator", Connection = "ServiceBusOptions")] Update tgMessages)
        {

            var keywordMessagesSender = new SendKeywordMessageToServiceBus();
            var generalMessagesSender = new SendGeneralMessageToServiceBus();

            keywordMessagesSender.SetNext(generalMessagesSender);

            keywordMessagesSender.SendMessageAsync(tgMessages);
        }
    }
}
