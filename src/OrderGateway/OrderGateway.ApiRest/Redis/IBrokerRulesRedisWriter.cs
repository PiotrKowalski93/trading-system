using OrderGateway.Core.Broker;

namespace OrderGateway.ApiRest.Redis
{
    public interface IBrokerRulesRedisWriter
    {
        Task SaveAsync(BrokerRules rules);
    }
}
