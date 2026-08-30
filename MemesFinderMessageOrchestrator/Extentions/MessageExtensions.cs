using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Extentions
{
    public static class MessageExtensions
    {
        public static string GetEffectiveText(this Message message) => message.Text ?? message.Caption;
    }
}
