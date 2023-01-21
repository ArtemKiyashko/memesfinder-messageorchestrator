using Azure;

namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IConversationAnalysisMessageClient
    {
        public Task<Response> GetConversationAnalysisAsync(string message);
    }
}
