using GrpcDotnetServer.Services;
using MarketData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

// Configure Kestrel from appsettings.json
builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection("Kestrel"));
});

var app = builder.Build();

app.MapGrpcService<MarketDataService>();
app.MapGet("/", () => "gRPC MarketData server running.");

app.Run();
