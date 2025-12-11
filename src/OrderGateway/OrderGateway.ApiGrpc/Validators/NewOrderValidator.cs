using OrderGateway.ApiGrpc.Caches;
using OrderGateway.ApiGrpc.Protos;

namespace OrderGateway.ApiGrpc.Validators
{
    public class NewOrderValidator : INewOrderValidator
    {
        private readonly IInMemoryInstrumentCache _instruments;
        private readonly IInMemoryBrokerRulesCache _brokerRules;

        //private readonly IRiskService _risk;
        //private readonly IOrderBook _orderBook;

        public NewOrderValidator(IInMemoryInstrumentCache instruments, IInMemoryBrokerRulesCache brokerRules)
        {
            _instruments = instruments;
            _brokerRules = brokerRules;
        }

        public NewOrderValidationResult Validate(NewOrderRequest req)
        {
            // --- Basic Checks ---
            if (string.IsNullOrWhiteSpace(req.ClientOrderId))
                return Failed("clientOrderId is required");

            if (string.IsNullOrWhiteSpace(req.Instrument))
                return Failed("instrument is required");

            if (req.Quantity <= 0)
                return Failed("quantity must be > 0");

            if (req.Price < 0)
                return Failed("price must be >= 0");

            if (req.Side != "BUY" && req.Side != "SELL")
                return Failed("side must be BUY or SELL");

            if (string.IsNullOrWhiteSpace(req.BrokerId))
                return Failed("brokerId is required");

            // --- Instrument Exists ---
            var inst = _instruments.Get(req.Instrument);
            if (inst == null)
                return Failed($"Unknown instrument: {req.Instrument}");

            // --- Price Bounds ---
            if (req.Price < inst.MinPrice || req.Price > inst.MaxPrice)
                return Failed($"Price {req.Price} outside allowed range {inst.MinPrice} - {inst.MaxPrice}");

            // --- Tick Size ---
            if (!inst.IsValidTick(req.Price))
                return Failed($"Price {req.Price} does not conform to tick size {inst.TickSize}");

            // --- Qty Bounds ---
            if (req.Quantity < inst.MinQuantity || req.Quantity > inst.MaxQuantity)
                return Failed($"Quantity {req.Quantity} outside allowed range {inst.MinQuantity} - {inst.MaxQuantity}");

            // BROKER RULES
            var rules = _brokerRules.Get(req.BrokerId);

            if (rules == null)
                return Failed($"Unknown broker {req.BrokerId}");

            // --- Allowed Instruments ---
            if (!rules.AllowedInstruments.Contains(req.Instrument))
                return Failed($"Instrument {req.Instrument} not allowed for broker {req.BrokerId}");

            // TODO: Add Order Type restrictions
            //if (req.OrderType == "MARKET" && !rules.AllowMarketOrders)
            //return Fail("Market orders not allowed for this broker");

            // --- Trading Hours ---
            var now = DateTime.UtcNow.TimeOfDay;
            if (now < rules.TradingStart || now > rules.TradingEnd)
                return Failed($"Order outside allowed trading hours for broker {req.BrokerId}");

            // --- Quantity Limit ---
            if (req.Quantity > rules.MaxQuantity)
                return Failed("Quantity exceeds broker max quantity");

            // TODO: Add validation with OrderBook

            return Passed();
        }

        private NewOrderValidationResult Failed(string reason) 
            => new NewOrderValidationResult(reason);

        private NewOrderValidationResult Passed()
            => new NewOrderValidationResult();
    }
}
