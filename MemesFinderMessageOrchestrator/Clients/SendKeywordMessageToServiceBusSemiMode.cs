using Azure.Messaging.ServiceBus;
using FluentValidation;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Factory;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Models;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send an object to the server in KeywordMessagesTopic if it contains a key word. Uses AI analysis if "мем" is found in a substring
    public class SendKeywordMessageToServiceBusSemiMode : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly IValidator<Message> _messageValidator;
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly IConversationAnalysisManager _analysisManager;


        public SendKeywordMessageToServiceBusSemiMode(ILogger<MessageOrchestrator> log,
            IServiceBusClient serviceBusClient,
            IOptions<ServiceBusOptions> serviceBusOptions,
            IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions,
            IValidator<Message> messageValidator,
            IConversationAnalysisManager analysisManager)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = serviceBusOptions.Value;
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
            _messageValidator = messageValidator;
            _analysisManager = analysisManager;
        }
        public override async Task SendMessageAsync(Update message)
        {
            Message incomeMessage = MessageProcessFactory.GetMessageToProcess(message);

            var messageValidationResult = _messageValidator.Validate(incomeMessage);

            if (!messageValidationResult.IsValid)
            {
                _logger.LogInformation(messageValidationResult.ToString());
                return;
            }

            if (!incomeMessage.Text.Contains("мем", StringComparison.OrdinalIgnoreCase))
            {
                await base.SendMessageAsync(message);
                return;
            }

            var messageResponse = await _analysisManager.AnalyzeMessage(
                incomeMessage.Text,
                _messageAnalysisClientOptions.TargetIntent,
                _messageAnalysisClientOptions.TargetCategory);

            if (!String.IsNullOrEmpty(messageResponse))
            {
                TgMessagesModels tgMessageModel = new TgMessagesModels
                {
                    Message = incomeMessage,
                    Keyword = messageResponse
                };

                await using ServiceBusSender sender = _serviceBusClient.CreateSender(_serviceBusOptions.KeywordMessagesTopic);
                ServiceBusMessage serviceBusMessage = new(tgMessageModel.ToJson());
                await sender.SendMessageAsync(serviceBusMessage);

                _logger.LogInformation($"Keyword extracted by AI: {messageResponse}");
            }
            else
            {
                await base.SendMessageAsync(message);
            }
        }

        public override bool SupportsMode(AnalysisMode mode)
        {
            return mode == AnalysisMode.SEMI_MODE;
        }
    }
}