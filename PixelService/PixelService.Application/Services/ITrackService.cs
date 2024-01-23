using PixelService.Contracts.Events;

namespace PixelService.Application.Services;

public interface ITrackService
{
    Task SendAsync(TrackEvent trackEvent);
}