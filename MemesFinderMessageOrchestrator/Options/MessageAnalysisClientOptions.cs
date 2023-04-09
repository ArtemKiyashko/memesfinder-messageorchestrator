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
        public string TargetIntent { get; set; }
        public string TargetCategory { get; set; }
        public int AnalysisMode { get; set; }
    }
}
