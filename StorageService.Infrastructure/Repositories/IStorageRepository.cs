using PixelService.Contracts.Events;

namespace StorageService.Infrastructure.Repositories;

public interface IStorageRepository
{
    Task SaveAsync(TrackEvent trackEvent);
}