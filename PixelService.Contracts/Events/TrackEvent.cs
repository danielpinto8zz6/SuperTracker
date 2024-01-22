namespace PixelService.Contracts.Events;

public class TrackEvent
{
    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public string Referrer { get; set; }
}