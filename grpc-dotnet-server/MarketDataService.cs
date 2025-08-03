using Grpc.Core;
using MarketData;

namespace GrpcDotnetServer.Services;

public class MarketDataService : MarketData.MarketDataBase
{
    public override async Task SubscribePrice(PriceRequest request, IServerStreamWriter<PriceUpdate> responseStream, ServerCallContext context)
    {
        var random = new Random();
        int count = 0;
        while (!context.CancellationToken.IsCancellationRequested && count++ < 100)
        {
            var priceUpdate = new PriceUpdate
            {
                Symbol = request.Symbol,
                Price = 100 + random.NextDouble() * 50,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            await responseStream.WriteAsync(priceUpdate);
            await Task.Delay(1000);
        }
    }
}
