using Azure;
using Azure.AI.Language.Conversations;
using Azure.Core;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;

namespace MemesFinderMessageOrchestrator.Clients
{
    internal class ConversationAnalysisMessageClient : IConversationAnalysisMessageClient
    {
        private readonly ConversationAnalysisClient _conversationAnalysisClient;
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;

        public ConversationAnalysisMessageClient(ConversationAnalysisClient conversationAnalysisClient, IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions)
        {
            _conversationAnalysisClient = conversationAnalysisClient;
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
        }
        public async Task<Response> GetConversationAnalysisAsync(string message)
        {
            var data = new ConversationRequestModel
            {
                AnalysisInput = new AnalysisInput
                {
                    ConversationItem = new ConversationItem
                    {
                        Text = message,
                        Id = "1",
                        ParticipantId = "1",
                        Modality = "text",
                        Language = _messageAnalysisClientOptions.Language
                    }
                },
                Parameters = new Parameters
                {
                    ProjectName = _messageAnalysisClientOptions.ProjectName,
                    DeploymentName = _messageAnalysisClientOptions.DeploymentName,
                    StringIndexType = _messageAnalysisClientOptions.StringIndexType
                },
                Kind = "Conversation",
            };

            Response response = await _conversationAnalysisClient.AnalyzeConversationAsync(RequestContent.Create(data));

            return response;
        }
    }
}
