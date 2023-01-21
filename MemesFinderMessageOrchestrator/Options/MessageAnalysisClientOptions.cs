using System;

namespace MemesFinderMessageOrchestrator.Options
{
    public class MessageAnalysisClientOptions
    {
        public Uri UriEndpoint { get; set; }
        public string ProjectName { get; set; }
        public string DeploymentName { get; set; }
        public string StringIndexType { get; set; }
        public string Language { get; set; }
        public string TargetKindMeme { get; set; }

    }
}
