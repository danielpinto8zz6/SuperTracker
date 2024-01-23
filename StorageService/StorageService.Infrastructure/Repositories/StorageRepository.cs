using System.Globalization;
using Microsoft.Extensions.Configuration;
using StorageService.Contracts.Events;

namespace StorageService.Infrastructure.Repositories;

public class StorageRepository : IStorageRepository
{
    private readonly string _storagePath;
    
    private const string DefaultPath = "/tmp/visits.log";
    
    public StorageRepository(IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        _storagePath = configuration["Storage:Path"] ?? DefaultPath;
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