namespace MarketData.Core
{
    public sealed class Order
    {
        public long OrderId;
        public Side Side;
        public decimal Price;
        public int Quantity;
    }
}
