using Moq;
using NUnit.Framework;
using PixelService.Application.Services;
using PixelService.Contracts.Events;

namespace PixelService.Application.UnitTests;

public class TrackServiceTests
{
    private Mock<IProducerService<TrackEvent>> _producerServiceMock;
    private TrackService trackService;

    [SetUp]
    public void Setup()
    {
        _producerServiceMock = new Mock<IProducerService<TrackEvent>>();
        trackService = new TrackService(_producerServiceMock.Object);
    }

    [Test]
    public async Task ShouldSendTrackEventToProducerServiceAsync()
    {
        var trackEvent = new TrackEvent
        {
            Referrer = "https://www.example.com",
            UserAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36",
            IpAddress = "192.168.1.1"
        };

        _producerServiceMock.Setup(x => x.ProduceAsync(It.IsAny<TrackEvent>())).Returns(Task.CompletedTask);

        await trackService.SendAsync(trackEvent);
    }

    [Test]
    public void ShouldThrowArgumentNullExceptionWhenTrackEventIsNull()
    {
        Assert.ThrowsAsync<ArgumentNullException>(async () => await trackService.SendAsync(null));
    }
}