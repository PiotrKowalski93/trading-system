using OrderGateway.ApiGrpc.Caches;
using OrderGateway.ApiGrpc.Repositories;
using OrderGateway.ApiGrpc.Services;
using OrderGateway.ApiGrpc.Validators;

namespace OrderGateway.ApiGrpc
{
    public class Program
    {
        private static WebApplication? _application;
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddGrpc();
            builder.Services.AddSingleton<IInMemoryOrderRepository, InMemoryOrderRepository>();
            builder.Services.AddSingleton<INewOrderValidator, NewOrderValidator>();
            builder.Services.AddSingleton<IInMemoryInstrumentCache, InMemoryInstrumentCache>();
            builder.Services.AddSingleton<IInMemoryBrokerRulesCache, InMemoryBrokerRulesCache>();

            _application = builder.Build();

            // Loach caches
            LoadInstruments();

            // Configure the HTTP request pipeline.
            _application.MapGrpcService<OrderService>();
            _application.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
            _application.Run();
        }

        // TODO: Refactor to load from DB or Redis
        private static void LoadInstruments()
        {
            var instrumentCache = _application.Services.GetRequiredService<IInMemoryInstrumentCache>();

            instrumentCache.AddOrUpdate(new InstrumentMetadata(
                "AAPL",
                0.01,
                1,
                1000,
                1,
                1_000_000,
                0.05 // 5%
            ));

            instrumentCache.AddOrUpdate(new InstrumentMetadata(
                "TSLA",
                0.01,
                1,
                1000,
                1,
                1_000_000,
                0.05 // 5%
            ));
        }

        private static void LoadBrokerRules()
        {

        }
    }
}