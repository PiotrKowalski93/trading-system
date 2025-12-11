using OrderGateway.ApiGrpc.Broker;

namespace OrderGateway.ApiGrpc.Caches
{
    public interface IInMemoryBrokerRulesCache
    {
        BrokerRules? Get(string brokerId);
        void AddOrUpdate(BrokerRules rules);
    }
}
