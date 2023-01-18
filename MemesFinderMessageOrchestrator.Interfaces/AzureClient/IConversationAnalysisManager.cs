using Telegram.Bot.Types;

namespace MMemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisManager
    {
        public Task<string> AnalyzeMessage(Message message, string targetKind);
    }
}