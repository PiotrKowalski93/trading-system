namespace MarketData.Core.ITCH
{
    /// <summary>
    /// Defining Msg Types in ITCH
    /// </summary>
    public static class MessageType
    {
        // Order Lifecycle
        public const byte SystemEvent = (byte)'S';
        public const byte StockDirectory = (byte)'D';
        public const byte StockTradingAction = (byte)'H';
        public const byte RegSHOShortSalePriceTest = (byte)'Y';
        public const byte RegSHOShortSaleQtyHld = (byte)'Q';
        public const byte AddOrder = (byte)'A';
        public const byte AddOrderWithAttribution = (byte)'F';
        public const byte OrderExecuted = (byte)'E';
        public const byte OrderExecutedWithPrice = (byte)'C';
        public const byte OrderCanceled = (byte)'X';
        public const byte OrderDeleted = (byte)'D';
        public const byte OrderReplaced = (byte)'U';
    }
}
