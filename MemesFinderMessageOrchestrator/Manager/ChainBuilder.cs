using MemesFinderMessageOrchestrator.Clients;
using System.Collections.Generic;

namespace MemesFinderMessageOrchestrator.Manager
{
    public class ChainBuilder
    {
        public ISendMessageToServiceBus BuildChain(IEnumerable<ISendMessageToServiceBus> serviceBusSenders)
        {
            ISendMessageToServiceBus chain = null;
            foreach (var serviceBusSender in serviceBusSenders)
            {
                if (chain == null)
                {
                    chain = serviceBusSender;
                }
                else
                {
                    chain.SetNext(serviceBusSender);
                }
            }
            return chain;
        }
    }
}
