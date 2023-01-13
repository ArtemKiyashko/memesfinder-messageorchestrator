using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    public abstract class AbstractSendMessagesToServiceBus : ISendMessageToServiceBus
    {
        private ISendMessageToServiceBus _next;

        public ISendMessageToServiceBus SetNext(ISendMessageToServiceBus next)
        {
            _next = next;
            return next;
        }

        public virtual async Task SendMessageAsync(Update message)
        {
            if (_next != null)
            {
                await _next.SendMessageAsync(message);
            }
        }
    }
}
