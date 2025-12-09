using OrderGateway.ApiGrpc.Caches;
using OrderGateway.ApiGrpc.Models;
using OrderGateway.ApiGrpc.Protos;
using System.ComponentModel.DataAnnotations;

namespace OrderGateway.ApiGrpc.Validators
{
    public class NewOrderValidator : INewOrderValidator
    {
        private readonly IInMemoryInstrumentCache _instruments;
        //private readonly IRiskService _risk;
        //private readonly IOrderBook _orderBook;

        public NewOrderValidator(IInMemoryInstrumentCache instruments)
        {
            _instruments = instruments;
        }

        public NewOrderValidationResult Validate(NewOrderRequest req)
        {
            // --- BASIC CHECKS ---
            if (string.IsNullOrWhiteSpace(req.ClientOrderId))
                return new NewOrderValidationResult("clientOrderId is required");

            if (string.IsNullOrWhiteSpace(req.Instrument))
                return new NewOrderValidationResult("instrument is required");

            if (req.Quantity <= 0)
                return new NewOrderValidationResult("quantity must be > 0");

            if (req.Price < 0)
                return new NewOrderValidationResult("price must be >= 0");

            if (req.Side != "BUY" && req.Side != "SELL")
                return new NewOrderValidationResult("side must be BUY or SELL");

            // --- INSTRUMENT EXISTS ---
            var inst = _instruments.Get(req.Instrument);
            if (inst == null)
                return new NewOrderValidationResult($"Unknown instrument: {req.Instrument}");

            // --- PRICE BOUNDS ---
            if (req.Price < inst.MinPrice || req.Price > inst.MaxPrice)
                return new NewOrderValidationResult($"Price {req.Price} outside allowed range {inst.MinPrice} - {inst.MaxPrice}");

            // --- TICK SIZE ---
            if (!inst.IsValidTick(req.Price))
                return new NewOrderValidationResult($"Price {req.Price} does not conform to tick size {inst.TickSize}");

            // --- QUANTITY BOUNDS ---
            if (req.Quantity < inst.MinQuantity || req.Quantity > inst.MaxQuantity)
                return new NewOrderValidationResult($"Quantity {req.Quantity} outside allowed range {inst.MinQuantity} - {inst.MaxQuantity}");

            // TODO: Add validation with OrderBook

            return new NewOrderValidationResult();
        }
    }

    public interface INewOrderValidator
    {
        NewOrderValidationResult Validate(NewOrderRequest order);
    }

    public class NewOrderValidationResult
    {
        public bool Success { get; }
        public bool Failure { get; }
        public string FailedMessage { get; }

        public NewOrderValidationResult(string FailedMessage = "")
        {
            this.FailedMessage = FailedMessage;

            if(FailedMessage != "")
            {
                Success = false;
                Failure = true;
            }
            else
            {
                Success = true;
                Failure = false;
            }
        }
    }
}
