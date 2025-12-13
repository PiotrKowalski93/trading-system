using OrderGateway.Core.Broker;
using StackExchange.Redis;

namespace OrderGateway.ApiRest.Redis
{
    public class BrokerRulesRedisWriter : IBrokerRulesRedisWriter
    {
        private IConnectionMultiplexer _redis;

        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;

        public BrokerRulesRedisWriter(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public async Task SaveAsync(BrokerRules rules)
        {
            string key = $"broker:{rules.BrokerId}";

            var entries = new HashEntry[]
            {
                new("AllowMarketOrders", rules.AllowMarketOrders),
                new("MaxQuantity", rules.MaxQuantity),
                new("TradingStart", rules.TradingStart.ToString()),
                new("TradingEnd", rules.TradingEnd.ToString()),
                new("AllowedInstruments", string.Join(",", rules.AllowedInstruments))
            };

            await _db.HashSetAsync(key, entries);

            // Notify gateways
            await _subscriber.PublishAsync(RedisChannel.Literal("broker-rules-updated"), rules.BrokerId);
        }
    }
}
