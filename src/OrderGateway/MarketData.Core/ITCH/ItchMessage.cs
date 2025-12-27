namespace MarketData.Core.ITCH
{
    /// <summary>
    /// Base class for all ITCH messages
    /// </summary>
    public abstract class ItchMessage
    {
        public byte Type { get; set; }
        public abstract int GetMessageLength();
        public abstract void Serialize(byte[] buffer, int offset);
        public abstract void Deserialize(byte[] buffer, int offset);
    }
}
