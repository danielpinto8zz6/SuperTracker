using System.Text.Json;
using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using PixelService.Contracts.Events;

namespace PixelService.Application.Services;

public class KafkaProducerService : IProducerService<TrackEvent>
{
    private readonly IProducer<string, string> _producer;
    private const string Topic = "track";

    public KafkaProducerService(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        var producerconfig = new ProducerConfig
        {
            BootstrapServers = configuration["Kafka:BootstrapServers"]
        };

        _producer = new ProducerBuilder<string, string>(producerconfig)
            .Build();
    }

    public Task ProduceAsync(TrackEvent trackEvent)
    {
        var message = new Message<string, string>
            { Key = Guid.NewGuid().ToString(), Value = JsonSerializer.Serialize(trackEvent) };

        return _producer.ProduceAsync(Topic, message);
    }
}