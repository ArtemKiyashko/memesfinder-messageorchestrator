using MemesFinderMessageOrchestrator.Extentions;
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
            builder.Services.AddMessageAnalyticsClient(_functionConfig);

            builder.Services.AddLogging();
        }
    }
}
