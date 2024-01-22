using Microsoft.AspNetCore.Mvc;
using PixelService.Application.Services;
using PixelService.Contracts.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ITrackService, TrackService>();
builder.Services.AddTransient<IProducerService<TrackEvent>, KafkaProducerService>();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/track", async (HttpRequest request, [FromServices] ITrackService trackService) =>
    {
        var ipAddress = request.HttpContext.Connection.RemoteIpAddress?.ToString();
        var userAgent = request.Headers["User-Agent"].ToString();
        var referrer = request.Headers["Referrer"].ToString();
        
        await trackService.SendAsync(new TrackEvent { IpAddress = ipAddress, Referrer = referrer, UserAgent = userAgent });

        return Results.File("logo.jpeg", contentType: "image/jpeg");
    })
    .WithName("GetTrack");

app.Run();