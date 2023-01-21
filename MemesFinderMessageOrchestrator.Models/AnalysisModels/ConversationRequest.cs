using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class ConversationRequestModel
    {
        [JsonPropertyName("analysisInput")]
        public AnalysisInput AnalysisInput { get; set; }

        [JsonPropertyName("parameters")]
        public Parameters Parameters { get; set; }

        [JsonPropertyName("kind")]
        public string Kind { get; set; }
    }
}

