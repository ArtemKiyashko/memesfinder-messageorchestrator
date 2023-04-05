using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Manager;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
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
        private readonly IConfiguration _configuration;

        public MessageOrchestrator(
            ILogger<MessageOrchestrator> log,
            IEnumerable<ISendMessageToServiceBus> serviceBusSender,
            IConfiguration configuration)
        {
            _logger = log;
            _serviceBusSender = serviceBusSender;
            _configuration = configuration;
        }

        [FunctionName("MessageOrchestrator")]
        public async Task Run([ServiceBusTrigger("allmessages", "orchestrator", Connection = "ServiceBusOptions")] Update tgMessages)
        {
            var analysisMode = (AnalysisMode)_configuration.GetValue<int>("AI_ANALYSIS_MODE");
            var chain = new ChainBuilder(analysisMode).BuildChain(_serviceBusSender);
            await chain.SendMessageAsync(tgMessages);
        }
    }
}
