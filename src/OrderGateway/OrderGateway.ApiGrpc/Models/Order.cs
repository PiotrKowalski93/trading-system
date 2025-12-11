namespace OrderGateway.ApiGrpc.Models
{
    public class Order
    {
        public string ClientOrderId { get; }
        public string Instrument { get; }
        public double Price { get; }
        public long Quantity { get; }
        public Side Side { get; }
        public string StrategyId { get; }

        public string BrokerId { get; }

        public Order(string clientOrderId,
            string instrument,
            double price,
            long quantity,
            Side side,
            string strategyId,
            string brokerId)
        {
            ClientOrderId = clientOrderId;
            Instrument = instrument;
            Price = price;
            Quantity = quantity;
            Side = side;
            StrategyId = strategyId;
            BrokerId = brokerId;
        }
    }
}
