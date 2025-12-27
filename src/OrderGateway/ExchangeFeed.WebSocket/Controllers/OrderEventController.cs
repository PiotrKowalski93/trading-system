using MarketData.Core;
using MarketData.Core.ITCH;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ExchangeFeed.WebSockets.Controllers
{
    [ApiController]
    [Route("/api/orderevent")]
    public class OrderEventController : ControllerBase
    {
        private readonly IBroadcaster _broadcaster;

        public OrderEventController(IBroadcaster broadcaster)
        {
            _broadcaster = broadcaster;
        }

        [HttpPost]
        public async Task<IActionResult> Publish([FromBody] OrderEvent orderEvent)
        {
            var stockSymbol = Encoding.ASCII.GetBytes(orderEvent.Stock);

            var addOrderMessage = new AddOrder
            {
                StockLocate = orderEvent.StockLocate,
                TrackingNumber = orderEvent.TrackingNumber,
                Timestamp = (ulong)DateTime.Now.Ticks,
                OrderReferenceNumber = orderEvent.OrderReferenceNumber,
                BuySellIndicator = (byte)orderEvent.BuySellIndicator,
                Shares = orderEvent.Shares,
                Stock = BitConverter.ToUInt32(stockSymbol),
                Price = orderEvent.Price
            };

            var payload = ItchSerializer.Serialize(addOrderMessage);
            await _broadcaster.BroadcastAsync(payload);

            return Ok("Published to market data feed");
        }
    }
}
