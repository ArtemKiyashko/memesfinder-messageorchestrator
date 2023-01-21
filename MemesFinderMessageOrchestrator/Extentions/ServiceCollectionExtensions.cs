using Azure.AI.Language.Conversations;
using Azure.Identity;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MemesFinderMessageOrchestrator.Extentions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServiceBusClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ServiceBusOptions>(configuration.GetSection("ServiceBusOptions"));

            services.AddAzureClients(clientBuilder =>
            {
                var provider = services.BuildServiceProvider();

                clientBuilder.UseCredential(new DefaultAzureCredential());
                clientBuilder.AddServiceBusClientWithNamespace(provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value.FullyQualifiedNamespace);
            });

            services.AddTransient<IServiceBusClient, ServiceBusMessagesClient>();

            return services;
        }

        public static IServiceCollection AddConversationAnalyticsClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MessageAnalysisClientOptions>(configuration.GetSection("MessageAnalysisClientOptions"));

            services.AddSingleton<ConversationAnalysisClient>(factory => new ConversationAnalysisClient(factory.GetRequiredService<IOptions<MessageAnalysisClientOptions>>().Value.UriEndpoint, new DefaultAzureCredential()));

            return services;
        }

    }
}

