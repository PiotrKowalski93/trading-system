using OrderGateway.ApiGrpc.Broker;
using System.Collections.Concurrent;

namespace OrderGateway.ApiGrpc.Caches
{
    public class InMemoryBrokerRulesCache : IInMemoryBrokerRulesCache
    {
        private readonly ConcurrentDictionary<string, BrokerRules> _cache = new();

        public void AddOrUpdate(BrokerRules rules)
        {
            _cache[rules.BrokerId] = rules;
        }

        public BrokerRules? Get(string brokerId)
        {
            return _cache.TryGetValue(brokerId, out var rules) ? rules : null;
        }
    }
}
