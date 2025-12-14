namespace OrderGateway.Core.Broker
{
    public class BrokerRules
    {
        public string BrokerId { get; set; }

        public HashSet<string> AllowedInstruments { get; set; }
        public bool AllowMarketOrders { get; set; }

        public long MaxQuantity { get; set; }

        public TimeSpan TradingStart { get; set; }
        public TimeSpan TradingEnd { get; set; }

        public BrokerRules()
        {
            
        }

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
