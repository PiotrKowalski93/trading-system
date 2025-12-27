using System.Buffers.Binary;

namespace MarketData.Core
{
    public static class MarketEventSerializer
    {
        public static byte[] Serialize(OrderEvent dto)
        {
            var buffer = new byte[64];
            var span = buffer.AsSpan();

            span[0] = (byte)dto.Type;
            BinaryPrimitives.WriteInt64LittleEndian(span[1..9], dto.OrderId);
            BinaryPrimitives.WriteInt64LittleEndian(span[9..17], dto.NewOrderId ?? 0);
            span[17] = (byte)dto.Side;
            BinaryPrimitives.WriteDoubleLittleEndian(span[18..26], (double)dto.Price);
            BinaryPrimitives.WriteInt32LittleEndian(span[26..30], dto.Quantity);

            return buffer;
        }

        public static MarketEvent Deserialize(ReadOnlySpan<byte> span)
        {
            return new MarketEvent
            {
                Type = (MarketDataEventType)span[0],
                OldOrderId = BinaryPrimitives.ReadInt64LittleEndian(span[1..9]),
                OrderId = BinaryPrimitives.ReadInt64LittleEndian(span[9..17]),
                Side = (Side)span[17],
                Price = (decimal)BinaryPrimitives.ReadDoubleLittleEndian(span[18..26]),
                Quantity = BinaryPrimitives.ReadInt32LittleEndian(span[26..30])
            };
        }
    }
}
