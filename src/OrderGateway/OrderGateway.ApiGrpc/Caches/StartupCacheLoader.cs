using StackExchange.Redis;
using IServer = StackExchange.Redis.IServer;

namespace OrderGateway.ApiGrpc.Caches
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
            // instruments
            var instrumentKeys = _server.Keys(pattern: "instrumentmetadata:*");
            foreach (var key in instrumentKeys)
            {
                //TODO: Parse
                var entries = await _db.HashGetAllAsync(key);
                //var inst = await LoadInstrument(key!);
                //_instrumentCache.AddOrUpdate(inst);
            }

            // brokers
            var brokerKeys = _server.Keys(pattern: "broker:*");
            foreach (var key in brokerKeys)
            {
                //TODO: Parse
                var entries = await _db.HashGetAllAsync(key);
                //var rules = await LoadBrokerRules(key!);
                //_brokers.AddOrUpdate(rules);
            }
        }
    }
}
