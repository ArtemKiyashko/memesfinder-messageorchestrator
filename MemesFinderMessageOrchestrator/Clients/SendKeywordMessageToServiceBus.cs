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
    //send object to the server if it contains a key word
    public class SendKeywordMessageToServiceBus : AbstractSendMessagesToServiceBus
    {
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IServiceBusClient _serviceBusClient;
        private readonly ServiceBusOptions _serviceBusOptions;
        private readonly IValidator<Message> _messageValidator;
        private readonly IKeywordExtractor _keywordExtractor;

        public SendKeywordMessageToServiceBus(ILogger<MessageOrchestrator> log,
            IServiceBusClient serviceBusClient,
            IOptions<ServiceBusOptions> serviceBusOptions,
            IValidator<Message> messageValidator,
            IKeywordExtractor keywordExtractor)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = serviceBusOptions.Value;
            _messageValidator = messageValidator;
            _keywordExtractor = keywordExtractor;
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

            string messageResponse = await _keywordExtractor.GetKeywordAsync(incomeMessage);

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