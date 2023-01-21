using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class Prediction
    {
        [JsonPropertyName("topIntent")]
        public string TopIntent { get; set; }

        [JsonPropertyName("projectKind")]
        public string ProjectKind { get; set; }

        [JsonPropertyName("intents")]
        public List<Intent> Intents { get; set; }

        [JsonPropertyName("entities")]
        public List<Entity> Entities { get; set; }
    }
}

