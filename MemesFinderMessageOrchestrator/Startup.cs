using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Interfaces.AzureClient;
using MemesFinderMessageOrchestrator.Options;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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

            builder.Services.AddScoped<KeywordExtractorFullMode>();
            builder.Services.AddScoped<KeywordExtractorSemiMode>();
            builder.Services.AddScoped<KeywordExtractorRegexMode>();

            builder.Services.AddScoped<IKeywordExtractor>((provider) =>
            {
                AnalysisMode mode = (AnalysisMode)_functionConfig.GetValue<int>("ANALYSIS_MODE");

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

            builder.Services.AddTransient<ISendMessageToServiceBus, SendGeneralMessageToServiceBus>();

            builder.Services.AddLogging();
        }
    }
}
