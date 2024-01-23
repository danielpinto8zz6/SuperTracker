using System.Net;
using Moq;
using NUnit.Framework;
using PixelService.Application.Services;
using PixelService.Contracts.Events;

namespace PixelService.Servce.UnitTests;

public class ServiceTests
{
    private HttpClient _httpClient;
    private Mock<IProducerService<TrackEvent>> _producerServiceMock;

    [SetUp]
    public void Setup()
    {
        _producerServiceMock = new Mock<IProducerService<TrackEvent>>();

        _httpClient = new HttpClient();
    }

    [Test]
    public async Task ShouldSendTrackEventAndReturnLogoImageAsync()
    {
        var ipAddress = "192.168.1.1";
        var userAgent = "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/98.0.4758.80 Safari/537.36";
        var referrer = "https://www.example.com";

        _producerServiceMock.Setup(x => x.ProduceAsync(It.IsAny<TrackEvent>())).Returns(Task.CompletedTask);

        var response = await _httpClient.GetAsync("/track");

        Assert.That(response.StatusCode == HttpStatusCode.OK);
        Assert.That(response.Content.Headers.ContentType.ToString(), Is.EqualTo("image/jpeg"));
    }

    [Test]
    public async Task ShouldLogErrorAndReturnBadRequestWhenTrackEventIsNull()
    {
        _producerServiceMock.Setup(x => x.ProduceAsync(It.IsAny<TrackEvent>())).ThrowsAsync(new ArgumentNullException("trackEvent"));

        var response = await _httpClient.GetAsync("/track");

        Assert.That(response.StatusCode == HttpStatusCode.BadRequest);
    }
}