namespace OrderGateway.ApiRest.Instruments
{
    public class AddInstrumentRequest
    {
        public string Symbol { get; set; }
        public double TickSize { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public long MinQuantity { get; set; }
        public long MaxQuantity { get; set; }
        public double MaxDeviationPercent { get; set; }
    }
}
