using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class AnalysisInput
    {
        [JsonPropertyName("conversationItem")]
        public ConversationItem ConversationItem { get; set; }
    }

}

