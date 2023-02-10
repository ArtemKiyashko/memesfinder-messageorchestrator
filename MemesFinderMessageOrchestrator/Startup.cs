using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Decorators;
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
            builder.Services.AddValidatorsFromAssemblyContaining<Startup>();

            if (_functionConfig.GetValue<bool>("ENABLE_AI_ANALYSIS"))
                builder.Services.AddConversationAnalyticsClient(_functionConfig);

            builder.Services.AddTransient<ISendMessageToServiceBus, SendGeneralMessageToServiceBus>();

            builder.Services.AddLogging();
        }
    }
}
