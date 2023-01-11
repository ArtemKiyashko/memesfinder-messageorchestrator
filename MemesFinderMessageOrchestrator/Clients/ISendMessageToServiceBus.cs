using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace MemesFinderMessageOrchestrator.Clients
{
    internal interface ISendMessageToServiceBus
    {
        Task SendMessageAsync(Update message);
    }
}
