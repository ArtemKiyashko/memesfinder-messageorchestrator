using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Manager;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(MemesFinderMessageOrchestrator.Startup))]
namespace MemesFinderMessageOrchestrator
{
    public class Startup : FunctionsStartup
    {
        private IConfigurationRoot _functionConfig;
        public override void Configure(IFunctionsHostBuilder builder)
        {
            _functionConfig = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddServiceBusClient(_functionConfig);
            builder.Services.AddConversationAnalyticsClient(_functionConfig);

            builder.Services.AddValidatorsFromAssemblyContaining<Startup>();

            builder.Services.AddTransient<IConversationAnalysisManager, ConversationAnalysisMessageManager>();
            builder.Services.AddTransient<IConversationAnalysisMessageClient, ConversationAnalysisMessageClient>();


            if (_functionConfig.GetValue<bool>("ENABLE_AI_ANALYSIS"))
                builder.Services.AddTransient<ISendMessageToServiceBus, SendKeywordMessageToServiceBus>();

            builder.Services.AddTransient<ISendMessageToServiceBus, SendGeneralMessageToServiceBus>();

            builder.Services.AddLogging();
        }
    }
}
