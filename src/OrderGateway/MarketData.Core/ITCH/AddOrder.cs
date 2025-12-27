namespace MarketData.Core.ITCH
{
    /// <summary>
    /// Add Order - 'A'
    /// Message Type (1 byte): 'A' (0x41) indicating Add Order
    /// Stock Locate(2 bytes): 0x0001 identifying the security
    /// Tracking Number(2 bytes) : 0x0003 for internal sequence tracking
    /// Timestamp(6 bytes) : Nanoseconds since midnight
    /// Order Reference Number(8 bytes): Unique order identifier
    /// Buy/Sell Indicator(1 byte): 'B' or 'S'
    /// Shares(4 bytes) : Order quantity as binary integer
    /// Stock Symbol(8 bytes): "AAPL    " (with padding)
    /// Price(4 bytes): Binary integer with implied 4 decimal places
    /// </summary>
    public partial class AddOrder : ItchMessage
    {
        public const int MessageSize = 36;

        public uint StockLocate { get; set; }
        public ushort TrackingNumber { get; set; }
        public ulong Timestamp { get; set; }
        public ulong OrderReferenceNumber { get; set; }
        public byte BuySellIndicator { get; set; }
        public uint Shares { get; set; }
        public uint Stock { get; set; }  // 4-bajtowy ticker
        public uint Price { get; set; }   // w pence'ach (dla ITCH 4.1)

        public AddOrder()
        {
            Type = MessageType.AddOrder;
        }

        public override int GetMessageLength() => MessageSize;

        public override void Serialize(byte[] buffer, int offset)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset + MessageSize > buffer.Length)
                throw new ArgumentException("Buffer too small");

            buffer[offset] = Type;
            BitConverter.EncodeUInt16(buffer, offset + 1, StockLocate);
            BitConverter.EncodeUInt16(buffer, offset + 3, TrackingNumber);
            BitConverter.EncodeUInt64(buffer, offset + 5, Timestamp);
            BitConverter.EncodeUInt64(buffer, offset + 13, OrderReferenceNumber);
            buffer[offset + 21] = BuySellIndicator;
            BitConverter.EncodeUInt32(buffer, offset + 22, Shares);
            BitConverter.EncodeUInt32(buffer, offset + 26, Stock);
            BitConverter.EncodeUInt32(buffer, offset + 30, Price);
        }

        public override void Deserialize(byte[] buffer, int offset)
        {
            if (buffer == null) throw new ArgumentNullException(nameof(buffer));
            if (offset + MessageSize > buffer.Length)
                throw new ArgumentException("Buffer too small");

            Type = buffer[offset];
            StockLocate = BitConverter.DecodeUInt16(buffer, offset + 1);
            TrackingNumber = BitConverter.DecodeUInt16(buffer, offset + 3);
            Timestamp = BitConverter.DecodeUInt64(buffer, offset + 5);
            OrderReferenceNumber = BitConverter.DecodeUInt64(buffer, offset + 13);
            BuySellIndicator = buffer[offset + 21];
            Shares = BitConverter.DecodeUInt32(buffer, offset + 22);
            Stock = BitConverter.DecodeUInt32(buffer, offset + 26);
            Price = BitConverter.DecodeUInt32(buffer, offset + 30);
        }
    }
}
