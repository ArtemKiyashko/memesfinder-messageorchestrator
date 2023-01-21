using Azure;

namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisClient
    {
        public Task<Response> GetConversationAnalysisAsync(string message);
    }
}
