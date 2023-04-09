using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    public class KeywordExtractorRegexMode : IKeywordExtractor
    {
        private readonly MessageAnalysisClientOptions _messageAnalysisClientOptions;
        private readonly string pattern = @"(?:\b\w+\s+)?\b(?:мем(?:ы|чик|асик)?)(?:\s+про)?\s*(.*)";

        public KeywordExtractorRegexMode(IOptions<MessageAnalysisClientOptions> messageAnalysisClientOptions)
        {
            _messageAnalysisClientOptions = messageAnalysisClientOptions.Value;
        }
        public async Task<string> GetKeywordAsync(Message incomeMessage)
        {
            if ((AnalysisMode)_messageAnalysisClientOptions.AnalysisMode == AnalysisMode.REGEX)
            {
                Match match = Regex.Match(incomeMessage.Text, pattern);

                if (!match.Success)
                {
                    return string.Empty;
                }
                var messageResponse = match.Groups[1].Value;

                return messageResponse;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}