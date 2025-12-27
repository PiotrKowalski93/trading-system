using MarketData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MarketData.ExchangeFeed
{
    public class MockExchangeFeed
    {
        private readonly ChannelWriter<MarketEvent> _writer;
        private long _orderId = 1;
        private readonly Random _rnd = new();

        public MockExchangeFeed(ChannelWriter<MarketEvent> writer)
        {
            _writer = writer;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                var side = _rnd.Next(2) == 0 ? Side.Bid : Side.Ask;
                var price = side == Side.Bid
                    ? 100m - _rnd.Next(5)
                    : 100m + _rnd.Next(5);

                var orderId = _rnd.Next(1, 25);

                var eventType = _rnd.Next(1, 6);

                // TODO: Init properly
                MarketEvent marketEvent = PrepareAddEvent(side, price, orderId); ;

                switch (eventType)
                {
                    case 1:
                        marketEvent = PrepareAddEvent(side, price, orderId);
                        break;
                    case 2:
                        marketEvent = PrepareExecuteEvent(side, price, orderId);
                        break;
                    case 3:
                        marketEvent = PrepareReduceEvent(side, price, orderId);
                        break;
                    case 4:
                        marketEvent = PrepareDeleteEvent(side, price, orderId);
                        break;
                    case 5:
                        marketEvent = PrepareReplaceEvent(side, price, orderId);
                        break;
                }

                await _writer.WriteAsync(marketEvent, ct);

                await Task.Delay(3000, ct); // symulacja feed rate
            }
        }


        private MarketEvent PrepareAddEvent(Side side, decimal price, int orderId)
        {
            return new MarketEvent(
                        MarketDataEventType.Add,
                        orderId,
                        null,
                        side,
                        price,
                        _rnd.Next(1, 20),
                        DateTime.Now.Ticks
                    );
        }

        private MarketEvent PrepareExecuteEvent(Side side, decimal price, int orderId)
        {
            return new MarketEvent(
                        MarketDataEventType.Execute,
                        orderId,
                        null,
                        side,
                        price,
                        _rnd.Next(1, 10),
                        DateTime.Now.Ticks
                    );
        }

        private MarketEvent PrepareReduceEvent(Side side, decimal price, int orderId)
        {
            return new MarketEvent(
                        MarketDataEventType.Reduce,
                        orderId,
                        null,
                        side,
                        price,
                        _rnd.Next(1, 10),
                        DateTime.Now.Ticks
                    );
        }

        private MarketEvent PrepareDeleteEvent(Side side, decimal price, int orderId)
        {
            return new MarketEvent(
                        MarketDataEventType.Delete,
                        orderId,
                        null,
                        side,
                        price,
                        _rnd.Next(1, 10),
                        DateTime.Now.Ticks
                    );
        }

        private MarketEvent PrepareReplaceEvent(Side side, decimal price, int orderId)
        {
            return new MarketEvent(
                        MarketDataEventType.Replace,
                        orderId,
                        orderId--,
                        side,
                        price,
                        _rnd.Next(1, 10),
                        DateTime.Now.Ticks
                    );
        }
    }
}
