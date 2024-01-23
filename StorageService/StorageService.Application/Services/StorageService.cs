using Microsoft.Extensions.Logging;
using StorageService.Contracts.Events;
using StorageService.Infrastructure.Repositories;

namespace StorageService.Application.Services;

public class StorageService : IStorageService
{
    private readonly ILogger<StorageService> _logger;
    private readonly IStorageRepository _storageRepository;
    
    public StorageService(ILogger<StorageService> logger, IStorageRepository storageRepository)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(storageRepository);
        _logger = logger;
        _storageRepository = storageRepository;
    }

    public Task SaveAsync(TrackEvent trackEvent)
    {
        if (string.IsNullOrWhiteSpace(trackEvent.IpAddress))
        {
            _logger.LogWarning("Ip address is invalid, ignoring event!");
            return Task.CompletedTask;
        }

        return _storageRepository.SaveAsync(trackEvent);
    }
}