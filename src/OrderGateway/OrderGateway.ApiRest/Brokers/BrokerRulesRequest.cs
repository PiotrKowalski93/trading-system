namespace OrderGateway.ApiRest.Brokers
{
    public class BrokerRulesRequest
    {
        public HashSet<string> AllowedInstruments { get; set; }
        public bool AllowMarketOrders { get; set; }
        public long MaxQuantity { get; set; }
        public TimeSpan TradingStart { get; set; }
        public TimeSpan TradingEnd { get; set; }
    }
}
