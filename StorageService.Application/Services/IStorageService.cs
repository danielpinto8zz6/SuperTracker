using PixelService.Contracts.Events;

namespace StorageService.Application.Services;

public interface IStorageService
{
    Task SaveAsync(TrackEvent trackEvent);
}