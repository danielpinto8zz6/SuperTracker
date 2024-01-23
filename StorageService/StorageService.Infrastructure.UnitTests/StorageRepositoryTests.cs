using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using StorageService.Contracts.Events;
using StorageService.Infrastructure.Repositories;

namespace StorageService.Infrastructure.UnitTests;

public class StorageRepositoryTests
{
    private Mock<IConfiguration> _configurationMock;
    private StorageRepository _storageRepository;

    [SetUp]
    public void Setup()
    {
        _configurationMock = new Mock<IConfiguration>();

        _configurationMock.Setup(config => config["Storage:Path"]).Returns("/tmp/visits.log");

        _storageRepository = new StorageRepository(_configurationMock.Object);
    }

    [Test]
    public async Task ShouldSaveTrackEventToFileAsync()
    {
        var trackEvent = new TrackEvent
        {
            Referrer = "https://www.example.com",
            UserAgent =
                "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36",
            IpAddress = "192.168.1.1"
        };

        await _storageRepository.SaveAsync(trackEvent);

        var fileContent = await File.ReadAllTextAsync("/tmp/visits.log");
        Assert.IsTrue(fileContent.Contains(trackEvent.Referrer));
        Assert.IsTrue(fileContent.Contains(trackEvent.UserAgent));
        Assert.IsTrue(fileContent.Contains(trackEvent.IpAddress));
    }
}