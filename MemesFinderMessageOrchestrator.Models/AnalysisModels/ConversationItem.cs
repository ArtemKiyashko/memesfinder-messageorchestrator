using System;
using System.Text.Json.Serialization;

namespace MemesFinderMessageOrchestrator.Models.AnalysisModels
{
    public class ConversationItem
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("participantId")]
        public string ParticipantId { get; set; }

        [JsonPropertyName("modality")]
        public string Modality { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }
    }
}

