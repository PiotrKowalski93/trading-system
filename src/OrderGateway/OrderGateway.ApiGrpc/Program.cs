using Microsoft.AspNetCore.Server.Kestrel.Core;
using OrderGateway.ApiGrpc.Caches;
using OrderGateway.ApiGrpc.Redis;
using OrderGateway.ApiGrpc.Repositories;
using OrderGateway.ApiGrpc.Services;
using OrderGateway.ApiGrpc.Validators;
using Prometheus;
using StackExchange.Redis;

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

            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect("localhost:6379")
            );
            builder.Services.AddSingleton<IInMemoryOrderRepository, InMemoryOrderRepository>();
            builder.Services.AddSingleton<INewOrderValidator, NewOrderValidator>();
            builder.Services.AddSingleton<IInMemoryInstrumentCache, InMemoryInstrumentCache>();
            builder.Services.AddSingleton<IInMemoryBrokerRulesCache, InMemoryBrokerRulesCache>();
            builder.Services.AddSingleton<IStartupCacheLoader, StartupCacheLoader>();

            builder.Services.AddHostedService<RedisConfigSubscriber>();

            builder.WebHost.ConfigureKestrel(options =>
            {
                // REST + /metrics
                options.ListenAnyIP(5100, o =>
                {
                    o.Protocols = HttpProtocols.Http1;
                });

                // gRPC
                options.ListenAnyIP(5101, o =>
                {
                    o.Protocols = HttpProtocols.Http2;
                });
            });

            _application = builder.Build();

            // Load Cache on startup
            FillCache();

            // Configure the HTTP request pipeline.
            _application.MapGrpcService<OrderService>();

            // Metrics
            _application.UseHttpMetrics();
            _application.UseGrpcMetrics();
            _application.MapMetrics("/metrics");
            
            _application.Run();
        }

        private static void FillCache()
        {
            var loader = _application.Services.GetService<IStartupCacheLoader>();
            loader.LoadAsync();
        }
    }
}