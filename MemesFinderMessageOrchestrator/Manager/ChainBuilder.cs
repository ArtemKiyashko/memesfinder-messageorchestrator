using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Options;
using System.Collections.Generic;

namespace MemesFinderMessageOrchestrator.Manager
{
    public class ChainBuilder
    {
        private readonly AnalysisMode _mode;

        public ChainBuilder(AnalysisMode mode)
        {
            _mode = mode;
        }
        public ISendMessageToServiceBus BuildChain(IEnumerable<ISendMessageToServiceBus> serviceBusSenders)
        {
            ISendMessageToServiceBus chain = null;
            foreach (var serviceBusSender in serviceBusSenders)
            {
                if (serviceBusSender.SupportsMode(_mode))
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
            }
            return chain;
        }
    }
}
