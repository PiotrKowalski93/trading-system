namespace OrderGateway.ApiGrpc.Broker
{
    public class BrokerRules
    {
        public string BrokerId { get; }

        public HashSet<string> AllowedInstruments { get; } = new();
        public bool AllowMarketOrders { get; } = true;

        public long MaxQuantity { get; } = 1_000_000;

        public TimeSpan TradingStart { get; } = TimeSpan.FromHours(8);
        public TimeSpan TradingEnd { get; } = TimeSpan.FromHours(22);

        public BrokerRules()
        {
             
        }
    }
}
