using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MemesFinderMessageOrchestrator.Factory
{
    public class MessageProcessFactory
    {
        public static Message GetMessageToProcess(Update tgUpdate) => tgUpdate.Type switch
        {
            UpdateType.Message => tgUpdate.Message,
            UpdateType.EditedMessage => tgUpdate.EditedMessage,
            _ => new Message()
        };
    }
}

