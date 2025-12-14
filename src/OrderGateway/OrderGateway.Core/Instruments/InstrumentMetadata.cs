namespace OrderGateway.Core.Instruments
{
    public class InstrumentMetadata
    {
        public string Symbol { get; set; }
        public double TickSize { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public long MinQuantity { get; set; }
        public long MaxQuantity { get; set; }

        // Fat-finger check
        public double MaxDeviationPercent { get; set; }

        public InstrumentMetadata()
        {
                
        }
        
        public InstrumentMetadata(
            string symbol,
            double tickSize,
            double minPrice,
            double maxPrice,
            long minQty,
            long maxQty,
            double maxDevPercent)
        {
            Symbol = symbol;
            TickSize = tickSize;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinQuantity = minQty;
            MaxQuantity = maxQty;
            MaxDeviationPercent = maxDevPercent;
        }

        public bool IsValidTick(double price)
        {
            double remainder = price % TickSize;
            return Math.Abs(remainder) < 1e-9
                || Math.Abs(remainder - TickSize) < 1e-9;
        }

        public bool IsWithinPriceBand(double price, double referencePrice)
        {
            if (referencePrice <= 0)
                return true;

            double lower = referencePrice * (1 - MaxDeviationPercent);
            double upper = referencePrice * (1 + MaxDeviationPercent);

            return price >= lower && price <= upper;
        }
    }
}
