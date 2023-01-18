using Azure;
using Azure.AI.Language.Conversations;
using Azure.Core;
using Azure.Identity;
using MemesFinderMessageOrchestrator.Options;
using MemesFinderMessagesOrchestrator.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MMemesFinderMessageOrchestrator.Interfaces.AzureClient;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using static MemesFinderMessagesOrchestrator.Models.ResponseConversationAnalysisClientModel;

namespace MemesFinderMessageOrchestrator.Manager
{
    public class ConversationAnalysisManager : IConversationAnalysisManager
    {
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly ILogger<MessageOrchestrator> _logger;

        public ConversationAnalysisManager(ILogger<MessageOrchestrator> log,
            IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions)
        {
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
            _logger = log;
        }
        public async Task<string> AnalyzeMessage(Message message, string targetKind)
        {

            DefaultAzureCredential credential = new DefaultAzureCredential();
            ConversationAnalysisClient clientAnalysis = new ConversationAnalysisClient(_messageAnalysisClientOptions.UriEndpoint, credential);

            var data = new RequestConversationAnalysisClientModel.ConversationRequestModel
            {
                AnalysisInput = new RequestConversationAnalysisClientModel.AnalysisInput
                {
                    ConversationItem = new RequestConversationAnalysisClientModel.ConversationItem
                    {
                        Text = message?.Text,
                        Id = "1",
                        ParticipantId = "1",
                        Modality = "text",
                        Language = _messageAnalysisClientOptions.Language
                    }
                },
                Parameters = new RequestConversationAnalysisClientModel.Parameters
                {
                    ProjectName = _messageAnalysisClientOptions.ProjectName,
                    DeploymentName = _messageAnalysisClientOptions.DeploymentName,
                    StringIndexType = _messageAnalysisClientOptions.StringIndexType
                },
                Kind = "Conversation",
            };

            Response response = clientAnalysis.AnalyzeConversation(RequestContent.Create(data));
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