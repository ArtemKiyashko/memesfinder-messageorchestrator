using Newtonsoft.Json;

namespace MemesFinderMessageOrchestrator.Extentions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object update) => update is null ? default : JsonConvert.SerializeObject(update);
    }
}
