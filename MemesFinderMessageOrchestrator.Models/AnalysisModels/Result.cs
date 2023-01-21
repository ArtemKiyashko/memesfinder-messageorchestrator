using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class Result
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("prediction")]
        public Prediction Prediction { get; set; }
    }
}

