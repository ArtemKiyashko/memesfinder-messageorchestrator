using Azure;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;
using System.Linq;
using FluentValidation;
using MemesFinderMessageOrchestrator.Extentions;

namespace MemesFinderMessageOrchestrator.Manager
{
    public class ConversationAnalysisMessageManager : IConversationAnalysisManager
    {
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly ILogger<MessageOrchestrator> _logger;
        private readonly IConversationAnalysisMessageClient _conversationAnalysisClient;
        private readonly IValidator<ConversationResponseModel> _validator;
        private const int MAX_MESSAGE_LENGTH = 1000;

        public ConversationAnalysisMessageManager(ILogger<MessageOrchestrator> log,
            IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions,
            IConversationAnalysisMessageClient conversationAnalysisClient,
            IValidator<ConversationResponseModel> validator)
        {
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
            _logger = log;
            _conversationAnalysisClient = conversationAnalysisClient;
            _validator = validator;
        }

        public async Task<string> AnalyzeMessage(string message, string targetIntent, string targetCategoty)
        {

            Response response = await _conversationAnalysisClient.GetConversationAnalysisAsync(message.LimitTo(MAX_MESSAGE_LENGTH));
            var model = response.Content.ToObjectFromJson<ConversationResponseModel>();

            var modelValidationResult = _validator.Validate(model);

            if (!modelValidationResult.IsValid)
                return null;

            if (!targetIntent.Equals(model?.Result?.Prediction?.TopIntent, System.StringComparison.OrdinalIgnoreCase))
                return null;

            var memeSubjectEntity = model.Result.Prediction.Entities
                .Where(entity => entity.Category.Equals(targetCategoty, System.StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(entity => entity.ConfidenceScore)
                .First();

            return memeSubjectEntity.Text;
        }
    }
}