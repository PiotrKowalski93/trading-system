namespace MarketData.Core.ITCH
{

public partial class AddOrder
    {
        /// <summary>
        /// Helpers for bit convertions (Big-Endian)
        /// </summary>
        public static class BitConverter
        {
            public static void EncodeUInt16(byte[] buffer, int offset, uint value)
            {
                buffer[offset] = (byte)((value >> 8) & 0xFF);
                buffer[offset + 1] = (byte)(value & 0xFF);
            }

            public static void EncodeUInt32(byte[] buffer, int offset, uint value)
            {
                buffer[offset] = (byte)((value >> 24) & 0xFF);
                buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
                buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
                buffer[offset + 3] = (byte)(value & 0xFF);
            }

            public static void EncodeUInt64(byte[] buffer, int offset, ulong value)
            {
                buffer[offset] = (byte)((value >> 56) & 0xFF);
                buffer[offset + 1] = (byte)((value >> 48) & 0xFF);
                buffer[offset + 2] = (byte)((value >> 40) & 0xFF);
                buffer[offset + 3] = (byte)((value >> 32) & 0xFF);
                buffer[offset + 4] = (byte)((value >> 24) & 0xFF);
                buffer[offset + 5] = (byte)((value >> 16) & 0xFF);
                buffer[offset + 6] = (byte)((value >> 8) & 0xFF);
                buffer[offset + 7] = (byte)(value & 0xFF);
            }

            public static ushort DecodeUInt16(byte[] buffer, int offset)
            {
                return (ushort)(((uint)buffer[offset] << 8) | buffer[offset + 1]);
            }

            public static uint DecodeUInt32(byte[] buffer, int offset)
            {
                return ((uint)buffer[offset] << 24) |
                       ((uint)buffer[offset + 1] << 16) |
                       ((uint)buffer[offset + 2] << 8) |
                       buffer[offset + 3];
            }

            public static ulong DecodeUInt64(byte[] buffer, int offset)
            {
                return ((ulong)buffer[offset] << 56) |
                       ((ulong)buffer[offset + 1] << 48) |
                       ((ulong)buffer[offset + 2] << 40) |
                       ((ulong)buffer[offset + 3] << 32) |
                       ((ulong)buffer[offset + 4] << 24) |
                       ((ulong)buffer[offset + 5] << 16) |
                       ((ulong)buffer[offset + 6] << 8) |
                       buffer[offset + 7];
            }
        }
    }
}
