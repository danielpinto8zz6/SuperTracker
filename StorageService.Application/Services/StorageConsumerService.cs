using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PixelService.Contracts.Events;

namespace StorageService.Application.Services;

public class StorageConsumerService : IHostedService
{
    private readonly IConsumer<string, string> _consumer;

    private readonly IStorageService _storageService;

    private readonly ILogger<StorageConsumerService> _logger;

    public StorageConsumerService(IConfiguration configuration, IStorageService storageService, ILogger<StorageConsumerService> logger)
    {
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(storageService);
        ArgumentNullException.ThrowIfNull(logger);

        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"],
            GroupId = "StorageService.Service.Service",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();
        _storageService = storageService;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _consumer.Subscribe("track");

        while (!cancellationToken.IsCancellationRequested)
        {
            await HandleMessageAsync(cancellationToken);
        }

        _consumer.Close();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _consumer.Dispose();

        return Task.CompletedTask;
    }

    private async Task HandleMessageAsync(CancellationToken cancellationToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(cancellationToken);

            _logger.LogInformation("Received track event, processing...");
            
            
            var trackEvent = JsonSerializer.Deserialize<TrackEvent>(consumeResult.Message.Value);

            await _storageService.SaveAsync(trackEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError($"Error processing Kafka message: {ex.Message}", ex);
        }
    }
}