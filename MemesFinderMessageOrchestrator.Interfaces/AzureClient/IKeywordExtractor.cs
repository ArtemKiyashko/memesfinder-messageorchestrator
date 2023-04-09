using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Interfaces.AzureClient
{
    public interface IKeywordExtractor
    {
        Task<string> GetKeywordAsync(Message message);
    }
}
