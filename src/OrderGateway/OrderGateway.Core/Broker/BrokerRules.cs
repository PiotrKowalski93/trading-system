namespace OrderGateway.Core.Broker
{
    public class BrokerRules
    {
        public string BrokerId { get; }

        public HashSet<string> AllowedInstruments { get; } = new();
        public bool AllowMarketOrders { get; }

        public long MaxQuantity { get; }

        public TimeSpan TradingStart { get; } = TimeSpan.FromHours(8);
        public TimeSpan TradingEnd { get; } = TimeSpan.FromHours(22);

        public BrokerRules(string brokerId,
            HashSet<string> allowedInstruments,
            bool allowMarketOrders,
            long maxQuantity,
            TimeSpan tradingStart,
            TimeSpan tradingEnd)
        {
            BrokerId = brokerId;
            AllowedInstruments = allowedInstruments;
            AllowMarketOrders = allowMarketOrders;
            MaxQuantity = maxQuantity;
            TradingStart = tradingStart;
            TradingEnd = tradingEnd;
        }
    }
}
