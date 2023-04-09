using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    public class KeywordExtractorSemiMode : IKeywordExtractor
    {
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly IConversationAnalysisManager _analysisManager;

        public KeywordExtractorSemiMode(IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions, IConversationAnalysisManager analysisManager)
        {
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
            _analysisManager = analysisManager;
        }
        public async Task<string> GetKeywordAsync(Message incomeMessage)
        {
            if ((AnalysisMode)_messageAnalysisClientOptions.AnalysisMode == AnalysisMode.SEMI_MODE)
            {
                if (!incomeMessage.Text.Contains("мем", StringComparison.OrdinalIgnoreCase))
                {
                    return string.Empty; ;
                }

                var messageResponse = await _analysisManager.AnalyzeMessage(
                    incomeMessage.Text,
                    _messageAnalysisClientOptions.TargetIntent,
                    _messageAnalysisClientOptions.TargetCategory);

                return messageResponse;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
