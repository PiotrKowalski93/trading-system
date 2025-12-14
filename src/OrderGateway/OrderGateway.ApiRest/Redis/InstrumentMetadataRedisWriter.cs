using OrderGateway.Core.Instruments;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderGateway.ApiRest.Redis
{
    public class InstrumentMetadataRedisWriter : IInstrumentMetadataRedisWriter
    {
        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;

        public InstrumentMetadataRedisWriter(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public async Task SaveAsync(InstrumentMetadata instrument)
        {
            string key = $"instrumentmetadata:{instrument.Symbol}";
            string value = JsonSerializer.Serialize(instrument);

            await _db.StringSetAsync(key, value);

            // Notify gateways
            await _subscriber.PublishAsync(RedisChannel.Literal("instrument-metadata-updated"), instrument.Symbol);
        }
    }

    public interface IInstrumentMetadataRedisWriter
    {
        Task SaveAsync(InstrumentMetadata rules);
    }
}
