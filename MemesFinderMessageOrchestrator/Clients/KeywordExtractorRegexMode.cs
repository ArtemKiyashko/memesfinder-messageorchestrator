using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    public class KeywordExtractorRegexMode : IKeywordExtractor
    {
        private readonly string pattern = @"(?:\b\w+\s+)?\b(?:мем(?:ы|чик|асик)?)(?:\s+про)?\s*(.*)";

        public async Task<string> GetKeywordAsync(Message incomeMessage)
        {

            Match match = Regex.Match(incomeMessage.Text, pattern);

            if (!match.Success)
            {
                return string.Empty;
            }
            var messageResponse = match.Groups[1].Value;

            return messageResponse;
        }
    }
}