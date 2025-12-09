using System.Collections.Concurrent;

namespace OrderGateway.ApiGrpc.Caches
{
    public class InMemoryInstrumentCache : IInMemoryInstrumentCache
    {
        private readonly ConcurrentDictionary<string, InstrumentMetadata> _cache = new();

        public InstrumentMetadata? Get(string symbol)
        {
            return _cache.TryGetValue(symbol, out var meta) ? meta : null;
        }

        public bool Exists(string symbol)
        {
            return _cache.ContainsKey(symbol);
        }

        public void AddOrUpdate(InstrumentMetadata meta)
        {
            _cache[meta.Symbol] = meta;
        }
    }
}
