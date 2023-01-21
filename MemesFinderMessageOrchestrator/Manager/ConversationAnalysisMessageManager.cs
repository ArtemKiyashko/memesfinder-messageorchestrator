using Azure;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MMemesFinderMessageOrchestrator.Interfaces.AzureClient;
using System.Threading.Tasks;
using static MemesFinderMessagesOrchestrator.Models.ResponseConversationAnalysisClientModel;

namespace MemesFinderMessageOrchestrator.Manager
{
    public class ConversationAnalysisMessageManager : IConversationAnalysisManager
    {
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IConversationAnalysisClient _conversationAnalysisClient;

        public ConversationAnalysisMessageManager(ILogger<MessageOrchestrator> log,
            IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions,
            IConversationAnalysisClient conversationAnalysisClient)
        {
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
            _logger = log;
            _conversationAnalysisClient = conversationAnalysisClient;
        }
        public async Task<string> AnalyzeMessage(string message, string targetKind)
        {

            Response response = await _conversationAnalysisClient.GetConversationAnalysisAsync(message);
            var model = response.Content.ToObjectFromJson<ConversationResponseModel>();

            //check that TopIntent is not null and has a name MemeRequest
            if (model?.Result?.Prediction?.TopIntent != null && model?.Result?.Prediction?.TopIntent == targetKind)
            {
                return model.Result.Prediction.Entities[0].Text;
            }
            return null;
        }
    }
}