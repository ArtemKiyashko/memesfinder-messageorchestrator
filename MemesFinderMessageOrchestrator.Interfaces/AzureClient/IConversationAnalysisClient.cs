namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisClient
    {
        public Task<string> GetConversationAnalysisAsync(string conversationId);
    }
}
