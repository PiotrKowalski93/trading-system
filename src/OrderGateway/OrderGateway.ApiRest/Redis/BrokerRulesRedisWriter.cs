using OrderGateway.Core.Broker;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderGateway.ApiRest.Redis
{
    public class BrokerRulesRedisWriter : IBrokerRulesRedisWriter
    {
        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;

        public BrokerRulesRedisWriter(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public async Task SaveAsync(BrokerRules rules)
        {
            string key = $"broker:{rules.BrokerId}";
            string value = JsonSerializer.Serialize(rules);

            await _db.StringSetAsync(key, value);

            // Notify gateways
            await _subscriber.PublishAsync(RedisChannel.Literal("broker-rules-updated"), rules.BrokerId);
        }
    }
}
