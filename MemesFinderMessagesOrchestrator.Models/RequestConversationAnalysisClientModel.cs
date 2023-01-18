using System.Text.Json.Serialization;

namespace MemesFinderMessagesOrchestrator.Models
{
    public class RequestConversationAnalysisClientModel
    {
        public class AnalysisInput
        {
            [JsonPropertyName("conversationItem")]
            public ConversationItem ConversationItem { get; set; }
        }

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

        public class Parameters
        {
            [JsonPropertyName("projectName")]
            public string ProjectName { get; set; }

            [JsonPropertyName("deploymentName")]
            public string DeploymentName { get; set; }

            [JsonPropertyName("stringIndexType")]
            public string StringIndexType { get; set; }
        }

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
}
