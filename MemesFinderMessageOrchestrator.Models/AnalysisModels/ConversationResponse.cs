using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class ConversationResponseModel
    {
        [JsonPropertyName("kind")]
        public string Kind { get; set; }

        [JsonPropertyName("result")]
        public Result Result { get; set; }
    }
}

