using Azure.Monitor.OpenTelemetry.Exporter;
using FluentValidation;
using MemesFinderMessageOrchestrator.Clients;
using MemesFinderMessageOrchestrator.Extentions;
using MemesFinderMessageOrchestrator.Models.AnalysisModels;
using MemesFinderMessageOrchestrator.Validators;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Azure.Functions.Worker.OpenTelemetry;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenTelemetry.Trace;
using Azure.Core.Serialization;

var builder = FunctionsApplication.CreateBuilder(args);

AppContext.SetSwitch("Azure.Experimental.EnableActivitySource", true);

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource("Azure.Messaging.ServiceBus.*"))
    .UseFunctionsWorkerDefaults()
    .UseAzureMonitorExporter();

builder.Services.Configure<WorkerOptions>(options =>
    options.Serializer = new NewtonsoftJsonObjectSerializer());

builder.Services.AddServiceBusClient(builder.Configuration);
builder.Services.AddValidatorsFromAssemblyContaining<ConversationResponseModelValidator>();
builder.Services.AddConversationAnalyticsClient(builder.Configuration);
builder.Services.AddTransient<ISendMessageToServiceBus, SendGeneralMessageToServiceBus>();

builder.Build().Run();
