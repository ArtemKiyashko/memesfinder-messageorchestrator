using System;
using System.Threading.Tasks;
using Azure;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using Microsoft.Extensions.Logging;

namespace MemesFinderMessageOrchestrator.Decorators
{
	public class ConversationAnalysisLoggerDecorator : IConversationAnalysisManager
    {
        private readonly IConversationAnalysisManager _decoratee;
        private readonly ILogger<MessageOrchestrator> _logger;

        public ConversationAnalysisLoggerDecorator(IConversationAnalysisManager decoratee, ILogger<MessageOrchestrator> logger)
		{
            _decoratee = decoratee;
            _logger = logger;
        }

        public async Task<string> AnalyzeMessage(string message, string targetIntent, string targetCategoty)
        {
            try
            {
                return await _decoratee.AnalyzeMessage(message, targetIntent, targetCategoty);
            }
            catch (RequestFailedException ex)
            {
                _logger.LogError(ex, "Error requesting LUIS:{0}", ex.Message);
                return null;
            }
        }
    }
}

