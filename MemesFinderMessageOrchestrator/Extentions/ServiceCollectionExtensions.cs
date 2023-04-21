using System;
using Azure.AI.Language.Conversations;
using Azure.Identity;
using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Decorators;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Manager;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;
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

            services.AddTransient<IConversationAnalysisManager>(factory =>
            {
                var options = factory.GetRequiredService<IOptions<MessageAnalysisClientOptions>>();
                var client = factory.GetRequiredService<IConversationAnalysisMessageClient>();
                var validator = factory.GetRequiredService<IValidator<ConversationResponseModel>>();
                return new ConversationAnalysisMessageManager(options, client, validator);
            });

            services.Decorate<IConversationAnalysisManager, ConversationAnalysisLoggerDecorator>();
            services.AddTransient<IConversationAnalysisMessageClient, ConversationAnalysisMessageClient>();
            services.AddTransient<ISendMessageToServiceBus, SendKeywordMessageToServiceBus>();
            
            services.AddScoped<KeywordExtractorFullMode>();
            services.AddScoped<KeywordExtractorSemiMode>();
            services.AddScoped<KeywordExtractorRegexMode>();

            services.AddScoped<IKeywordExtractor>((provider) =>
            {
                AnalysisMode mode = (AnalysisMode)configuration.GetValue<int>("ANALYSIS_MODE");

                switch (mode)
                {
                    case AnalysisMode.FULL_MODE:
                        return provider.GetService<KeywordExtractorFullMode>();
                    case AnalysisMode.SEMI_MODE:
                        return provider.GetService<KeywordExtractorSemiMode>();
                    case AnalysisMode.REGEX:
                        return provider.GetService<KeywordExtractorRegexMode>();
                    default:
                        throw new InvalidOperationException($"Unsupported AnalysisMode: {mode}");
                }
            });

            return services;
        }

    }
}

