namespace MarketData.Core
{
    public class OrderEvent
    {
        public uint StockLocate { get; set; }
        public ushort TrackingNumber { get; set; }
        public ulong OrderReferenceNumber { get; set; }
        public char BuySellIndicator { get; set; } // 'B', 'S'
        public uint Shares { get; set; }
        public string Stock { get; set; }  // 4bites ticker
        public uint Price { get; set; } 
    }
}
