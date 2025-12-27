namespace MarketData.Core.ITCH
{
    public static class ItchSerializer
    {
        /// <summary>
        /// Serializes ITCH message to byte[]
        /// </summary>
        public static byte[] Serialize(ItchMessage message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            byte[] buffer = new byte[message.GetMessageLength()];
            message.Serialize(buffer, 0);
            return buffer;
        }

        /// <summary>
        /// Deserializes ITCH message from byte[]
        /// </summary>
        public static ItchMessage Deserialize(byte[] buffer, int offset = 0)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset >= buffer.Length)
                throw new ArgumentException("Offset exceeds buffer length");

            byte messageType = buffer[offset];
            ItchMessage message = CreateMessage(messageType);

            if (message == null)
                throw new InvalidOperationException($"Unknown message type: {(char)messageType}");

            message.Deserialize(buffer, offset);
            return message;
        }

        /// <summary>
        /// Creates message based on the messageType
        /// </summary>
        private static ItchMessage CreateMessage(byte messageType)
        {
            switch (messageType)
            {
                case (byte)MessageType.AddOrder:
                    return new AddOrder();

                //case (byte)MessageType.OrderExecuted:
                //    return new OrderExecuted();

                //case (byte)MessageType.OrderExecutedWithPrice:
                //    return new OrderExecutedWithPrice();

                //case (byte)MessageType.OrderCanceled:
                //    return new OrderCanceled();

                //case (byte)MessageType.OrderDeleted:
                //    return new OrderDeleted();

                //case (byte)MessageType.OrderReplaced:
                //    return new OrderReplaced();

                default:
                    return null;
            }
        }

        /// <summary>
        /// Serialize message to the exsiting buffer
        /// </summary>
        public static int Serialize(ItchMessage message, byte[] buffer, int offset)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            int messageLength = message.GetMessageLength();
            if (offset + messageLength > buffer.Length)
                throw new ArgumentException("Buffer too small to serialize message");

            message.Serialize(buffer, offset);
            return messageLength;
        }
    }
}
