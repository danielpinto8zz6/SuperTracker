using StorageService.Application.Services;
using StorageService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<IStorageService, StorageService.Application.Services.StorageService>();
builder.Services.AddTransient<IStorageRepository, StorageRepository>();
builder.Services.AddHostedService<StorageConsumerService>();

var app = builder.Build();

app.Run();