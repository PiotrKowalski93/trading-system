namespace MarketData.Core
{
    public readonly record struct MarketEvent(
        MarketDataEventType Type,
        long OrderId,
        long? OldOrderId,
        Side Side,
        decimal Price,
        int Quantity,
        long Timestamp
    );
}
