using OrderGateway.ApiGrpc.Caches;
using OrderGateway.Core.Broker;
using OrderGateway.Core.Instruments;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderGateway.ApiGrpc.Redis
{
    public class RedisConfigSubscriber : BackgroundService
    {
        private readonly ISubscriber _sub;
        private readonly IDatabase _db;
        private readonly IInMemoryInstrumentCache _instruments;
        private readonly IInMemoryBrokerRulesCache _brokers;

        public RedisConfigSubscriber(IConnectionMultiplexer redis,
             IInMemoryInstrumentCache instruments,
             IInMemoryBrokerRulesCache brokers)
        {
            _sub = redis.GetSubscriber();
            _db = redis.GetDatabase();
            _instruments = instruments;
            _brokers = brokers;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _sub.SubscribeAsync("instrument-metadata-updated", async (_, symbol) =>
            {
                if (symbol.HasValue && !symbol.IsNullOrEmpty) 
                {
                    var newMetadata = await _db.StringGetAsync("instrumentmetadata:" + symbol.ToString());
                    _instruments.AddOrUpdate(JsonSerializer.Deserialize<InstrumentMetadata>(newMetadata));
                }
            });

            await _sub.SubscribeAsync("broker-rules-updated", async (_, brokerId) =>
            {
                if (brokerId.HasValue && !brokerId.IsNullOrEmpty)
                {
                    var newBrokerRules = await _db.StringGetAsync("broker:" + brokerId.ToString());
                    _brokers.AddOrUpdate(JsonSerializer.Deserialize<BrokerRules>(newBrokerRules));
                }
            });
        }
    }
}
