using OrderGateway.ApiGrpc.Caches;
using OrderGateway.Core.Broker;
using OrderGateway.Core.Instruments;
using StackExchange.Redis;
using System.Text.Json;
using IServer = StackExchange.Redis.IServer;

namespace OrderGateway.ApiGrpc.Redis
{
    public class StartupCacheLoader : IStartupCacheLoader
    {
        private readonly IDatabase _db;
        private readonly IServer _server;
        private readonly IInMemoryInstrumentCache _instrumentCache;
        private readonly IInMemoryBrokerRulesCache _brokerRulesCache;

        public StartupCacheLoader(
            IConnectionMultiplexer redis,
            IInMemoryInstrumentCache instrumentCache,
            IInMemoryBrokerRulesCache brokerRulesCache)
        {
            _server = redis.GetServer(redis.GetEndPoints().First());
            _db = redis.GetDatabase();
            _instrumentCache = instrumentCache;
            _brokerRulesCache = brokerRulesCache;
        }

        public async Task LoadAsync()
        {
            try
            {
                // instruments
                var instrumentKeys = _server.Keys(pattern: "instrumentmetadata:*");
                foreach (var key in instrumentKeys)
                {
                    var value = await _db.StringGetAsync(key);
                    _instrumentCache.AddOrUpdate(JsonSerializer.Deserialize<InstrumentMetadata>(value));
                }

                // brokers
                var brokerKeys = _server.Keys(pattern: "broker:*");
                foreach (var key in brokerKeys)
                {
                    var value = await _db.StringGetAsync(key);
                    _brokerRulesCache.AddOrUpdate(JsonSerializer.Deserialize<BrokerRules>(value));
                }
            }
            catch (Exception ex)
            {
                //TODO: Add logging
                throw;
            }
        }
    }
}
