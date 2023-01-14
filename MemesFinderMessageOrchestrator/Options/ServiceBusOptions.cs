namespace MemesFinderMessageOrchestrator.Options
{
    public class ServiceBusOptions
    {
        public string FullyQualifiedNamespace { get; set; }
        public string GeneralMessagesTopic { get; set; }
        public string KeywordMessagesTopic { get; set; }
    }
}

