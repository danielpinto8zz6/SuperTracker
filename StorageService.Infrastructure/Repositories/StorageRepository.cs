using System.Globalization;
using Microsoft.Extensions.Configuration;
using PixelService.Contracts.Events;

namespace StorageService.Infrastructure.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly string _storagePath;

    public StorageRepository(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _storagePath = configuration["Storage:Path"] ??
                      throw new InvalidOperationException("Missing path configuration");
    }

    public Task SaveAsync(TrackEvent trackEvent)
    {
        using (var w = File.AppendText(_storagePath))
        {
            w.WriteLine($"{CurrentDateInIsoFormat}|{trackEvent.Referrer}|{trackEvent.UserAgent}|{trackEvent.IpAddress}");
        }
        
        return Task.CompletedTask;
    }

    private static string CurrentDateInIsoFormat
        => DateTime.UtcNow.ToString("o", CultureInfo.InvariantCulture);
}