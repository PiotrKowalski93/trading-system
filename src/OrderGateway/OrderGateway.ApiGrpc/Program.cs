using Microsoft.OpenApi.Models;
using OrderGateway.ApiGrpc.Broker;
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

            // Load caches
            LoadInstruments();
            LoadBrokerRules();

            // Configure the HTTP request pipeline.
            _application.MapGrpcService<OrderService>();
            //_application.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
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
            var brokerRulesCache = _application.Services.GetService<IInMemoryBrokerRulesCache>();

            brokerRulesCache.AddOrUpdate(new BrokerRules(
                brokerId: "BRK-001",
                allowedInstruments: new HashSet<string> { "AAPL", "MSFT", "GOOG" },
                allowMarketOrders: true,
                maxQuantity: 10_000,
                tradingStart: TimeSpan.FromHours(9),
                tradingEnd: TimeSpan.FromHours(21)
            ));

            brokerRulesCache.AddOrUpdate(new BrokerRules(
                brokerId: "BRK-002",
                allowedInstruments: new HashSet<string> { "BTC-USD", "ETH-USD" },
                allowMarketOrders: false,              // np. crypto broker tylko limit orders
                maxQuantity: 5,
                tradingStart: TimeSpan.FromHours(0),   // 24/7
                tradingEnd: TimeSpan.FromHours(24)
            ));

            brokerRulesCache.AddOrUpdate(new BrokerRules(
                brokerId: "BRK-003",
                allowedInstruments: new HashSet<string> { "EURUSD", "USDJPY", "GBPUSD" },
                allowMarketOrders: true,
                maxQuantity: 1_000_000,
                tradingStart: TimeSpan.FromHours(7),
                tradingEnd: TimeSpan.FromHours(17)     // np. broker FX działający w określonych sesjach
            ));

            brokerRulesCache.AddOrUpdate(new BrokerRules(
                brokerId: "BRK-004",
                allowedInstruments: new HashSet<string> { "TSLA" },
                allowMarketOrders: false,
                maxQuantity: 100,
                tradingStart: TimeSpan.FromHours(8),
                tradingEnd: TimeSpan.FromHours(22)
            ));
        }
    }
}