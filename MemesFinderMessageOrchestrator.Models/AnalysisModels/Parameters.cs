using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class Parameters
    {
        [JsonPropertyName("projectName")]
        public string ProjectName { get; set; }

        [JsonPropertyName("deploymentName")]
        public string DeploymentName { get; set; }

        [JsonPropertyName("stringIndexType")]
        public string StringIndexType { get; set; }
    }
}

