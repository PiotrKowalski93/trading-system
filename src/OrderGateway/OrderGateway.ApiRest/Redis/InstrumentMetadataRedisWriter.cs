using OrderGateway.Core.Instruments;
using StackExchange.Redis;

namespace OrderGateway.ApiRest.Redis
{
    public class InstrumentMetadataRedisWriter : IInstrumentMetadataRedisWriter
    {
        private IConnectionMultiplexer _redis;

        private readonly IDatabase _db;
        private readonly ISubscriber _subscriber;

        public InstrumentMetadataRedisWriter(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
            _subscriber = redis.GetSubscriber();
        }

        public async Task SaveAsync(InstrumentMetadata instrument)
        {
            string key = $"instrumentmetadata:{instrument.Symbol}";

            var entries = new HashEntry[]
            {
                new("Symbol", instrument.Symbol),
                new("TickSize", instrument.TickSize),
                new("MinPrice", instrument.MinPrice),
                new("MaxPrice", instrument.MaxPrice),
                new("MinQuantity", instrument.MinQuantity),
                new("MaxQuantity", instrument.MaxQuantity),
                new("MaxDeviationPercent", instrument.MaxDeviationPercent)
            };

            await _db.HashSetAsync(key, entries);

            // Notify gateways
            await _subscriber.PublishAsync(RedisChannel.Literal("instrument-metadata-updated"), instrument.Symbol);
        }
    }

    public interface IInstrumentMetadataRedisWriter
    {
        Task SaveAsync(InstrumentMetadata rules);
    }
}
