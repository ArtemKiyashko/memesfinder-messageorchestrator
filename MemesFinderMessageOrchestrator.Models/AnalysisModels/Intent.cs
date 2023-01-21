using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class Intent
    {
        [JsonPropertyName("category")]
        public string Category { get; set; }

        [JsonPropertyName("confidenceScore")]
        public double ConfidenceScore { get; set; }
    }
}

