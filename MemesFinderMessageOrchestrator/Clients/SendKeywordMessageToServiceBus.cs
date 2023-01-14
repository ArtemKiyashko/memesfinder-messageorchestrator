using Azure;
using Azure.AI.Language.Conversations;
using Azure.Identity;
using Azure.Messaging.ServiceBus;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Factory;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;

        public SendKeywordMessageToServiceBus(ILogger<MessageOrchestrator> log, IServiceBusClient serviceBusClient, IOptions<ServiceBusOptions> serviceBusOptions, IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions)
        {
            _logger = log;
            _serviceBusClient = serviceBusClient;
            _serviceBusOptions = serviceBusOptions.Value;
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
        }
        public override async Task SendMessageAsync(Update message)
        {
            var messageResponce = RequestToConversationAnalysisClientAsync(message);

            if (messageResponce.Result.topIntent("MemeRequest"))
            {
                await using ServiceBusSender sender = _serviceBusClient.CreateSender(_serviceBusOptions.KeywordMessagesTopic);
                ServiceBusMessage serviceBusMessage = new(message.ToJson());
                await sender.SendMessageAsync(serviceBusMessage);
                _logger.LogInformation($"Message with id {message.Message.MessageId} was sent to the keyword topic");
            }
            else
            {
                await base.SendMessageAsync(message);
            }
        }
        //request to ConversationAnalysisClient
        public async Task<Response> RequestToConversationAnalysisClientAsync(Update message)
        {
            DefaultAzureCredential credential = new DefaultAzureCredential();
            ConversationAnalysisClient clientAnalysis = new ConversationAnalysisClient(_messageAnalysisClientOptions.UriEndpoint, credential);
            Message incomeMessage = MessageProcessFactory.GetMessageToProcess(message);
            Response response = await clientAnalysis.AnalyzeConversationAsync(incomeMessage.Text);
            return response;
        }
    }


}