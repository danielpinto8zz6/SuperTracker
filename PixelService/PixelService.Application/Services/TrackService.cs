using PixelService.Contracts.Events;

namespace PixelService.Application.Services;

public class TrackService : ITrackService
{
    private readonly IProducerService<TrackEvent> _producerService;

    public TrackService(IProducerService<TrackEvent> producerService)
    {
        ArgumentNullException.ThrowIfNull(producerService);
        _producerService = producerService;
    }

    public Task SendAsync(TrackEvent trackEvent)
    {
        ArgumentNullException.ThrowIfNull(trackEvent);
        
        return _producerService.ProduceAsync(trackEvent);
    }
}