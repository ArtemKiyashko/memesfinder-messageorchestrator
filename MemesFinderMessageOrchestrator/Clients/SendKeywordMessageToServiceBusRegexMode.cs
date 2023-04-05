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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    //send an object to the server in KeywordMessagesTopic if it contains a key word. Uses a regular expression to search for a keyword.
    public class SendKeywordMessageToServiceBusRegexMode : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly IValidator<Message> _messageValidator;
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly IConversationAnalysisManager _analysisManager;
        private readonly string pattern = @"(?:\b\w+\s+)?\b(?:мем(?:ы|чик|асик)?)(?:\s+про)?\s*(.*)";


        public SendKeywordMessageToServiceBusRegexMode(ILogger<MessageOrchestrator> log,
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

            Match match = Regex.Match(incomeMessage.Text, pattern);
            if (!match.Success)
            {
                await base.SendMessageAsync(message);
                return;
            }
            var messageResponse = match.Groups[1].Value;

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
            return mode == AnalysisMode.REGEX;
        }
    }
}