using Azure.Monitor.OpenTelemetry.Exporter;
using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;
using MemesFinderMessageOrchestrator.Validators;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;

var builder = FunctionsApplication.CreateBuilder(args);

AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource("Azure.Messaging.ServiceBus.*"))
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

builder.Services.AddServiceBusClient(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<ConversationResponseModelValidator>();
builder.Services.AddConversationAnalyticsClient(builder.Configuration);
builder.Services.AddTransient<ISendMessageToServiceBus, SendGeneralMessageToServiceBus>();

builder.Build().Run();
