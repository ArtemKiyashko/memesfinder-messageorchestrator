using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Manager;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator
{
    public class MessageOrchestrator
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IEnumerable<ISendMessageToServiceBus> _serviceBusSender;

        public MessageOrchestrator(
            ILogger<MessageOrchestrator> log,
            IEnumerable<ISendMessageToServiceBus> serviceBusSender)
        {
            _logger = log;
            _serviceBusSender = serviceBusSender;
        }

        [Function("MessageOrchestrator")]
        public async Task Run([ServiceBusTrigger("allmessages", "orchestrator", Connection = "ServiceBusOptions")] Update tgMessages)
        {
            var chain = new ChainBuilder().BuildChain(_serviceBusSender);
            await chain.SendMessageAsync(tgMessages);
        }
    }
}
