using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using StorageService.Contracts.Events;
using StorageService.Infrastructure.Repositories;

namespace StorageService.Application.Tests;

public class StorageServiceTests
{
    private Mock<ILogger<Services.StorageService>> _loggerMock;
    private Mock<IStorageRepository> _storageRepositoryMock;
    private Services.StorageService _storageService;

    [SetUp]
    public void Setup()
    {
        _loggerMock = new Mock<ILogger<Services.StorageService>>();
        _storageRepositoryMock = new Mock<IStorageRepository>();
        _storageService = new Services.StorageService(_loggerMock.Object, _storageRepositoryMock.Object);
    }

    [Test]
    public async Task ShouldSaveEventWithValidIpAddressAsync()
    {
        var trackEvent = new TrackEvent
        {
            IpAddress = "192.168.1.1"
        };

        await _storageService.SaveAsync(trackEvent);

        _storageRepositoryMock.Verify(x => x.SaveAsync(trackEvent), Times.Once);
    }

    [Test]
    public async Task ShouldReturnWhenIpAddressIsInvalid()
    {
        var trackEvent = new TrackEvent
        {
            IpAddress = null
        };

        await _storageService.SaveAsync(trackEvent);

        _storageRepositoryMock.Verify(x => x.SaveAsync(trackEvent), Times.Never);
    }
}