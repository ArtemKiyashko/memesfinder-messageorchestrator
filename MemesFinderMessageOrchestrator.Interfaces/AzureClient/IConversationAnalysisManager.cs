namespace MMemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisManager
    {
        public Task<string> AnalyzeMessage(string message, string targetKind);
    }
}