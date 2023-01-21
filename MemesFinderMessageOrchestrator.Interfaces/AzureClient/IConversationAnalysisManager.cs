namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisManager
    {
        public Task<string> AnalyzeMessage(string message, string targetIntent, string targetCategoty);
    }
}