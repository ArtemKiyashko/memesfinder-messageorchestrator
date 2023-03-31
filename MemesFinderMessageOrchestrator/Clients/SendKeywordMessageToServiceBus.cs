using Azure.Messaging.ServiceBus;
using FluentValidation;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Factory;
using MemesFinderMessageOrchestrator.Options;
using MemesFinderMessageOrchestrator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send object to the server if it contains a key word
    public class SendKeywordMessageToServiceBus : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly IValidator<Message> _messageValidator;
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly IConversationAnalysisManager _analysisManager;


        public SendKeywordMessageToServiceBus(ILogger<MessageOrchestrator> log,
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
    }
}