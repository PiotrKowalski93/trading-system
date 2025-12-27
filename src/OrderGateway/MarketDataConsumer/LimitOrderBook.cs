using MarketData.Core;
using MarketData.Core.ITCH;

namespace MarketData.Consumer
{
    public class LimitOrderBook
    {
        // orderId → Order
        private readonly Dictionary<long, Order> _orders = new();

        private readonly SortedDictionary<decimal, int> _bids = new(Comparer<decimal>.Create((a, b) => b.CompareTo(a)));
        private readonly SortedDictionary<decimal, int> _asks = new();

        public void Apply(ItchMessage evt)
        {
            switch (evt.Type)
            {
                case (byte)MessageType.AddOrder:
                    //OnAdd(evt);
                    Console.WriteLine("Add Order Recieved");
                    break;

                //case MarketDataEventType.Execute:
                //    //OnExecute(evt);
                //    break;

                //case MarketDataEventType.Reduce:
                //    //OnReduce(evt);
                //    break;

                //case MarketDataEventType.Delete:
                //    //OnDelete(evt.OrderId);
                //    break;

                //case MarketDataEventType.Replace:
                //    //OnReplace(evt);
                //    break;
            }
        }

        private void OnAdd(MarketEvent evt)
        {
            var order = new Order
            {
                OrderId = evt.OrderId,
                Side = evt.Side,
                Price = evt.Price,
                Quantity = evt.Quantity
            };

            _orders[order.OrderId] = order;

            var book = order.Side == Side.Bid ? _bids : _asks;
            book.TryGetValue(order.Price, out var qty);
            book[order.Price] = qty + order.Quantity;

            Console.WriteLine($"ADD | {order.OrderId} | {order.Side} | {order.Quantity}@{order.Price}");
        }

        private void OnExecute(MarketEvent evt)
        {
            if (!_orders.TryGetValue(evt.OrderId, out var order))
                return;

            var executedQty = evt.Quantity;
            order.Quantity -= executedQty;

            var book = order.Side == Side.Bid ? _bids : _asks;
            book[order.Price] -= executedQty;

            if (book[order.Price] <= 0)
                book.Remove(order.Price);

            Console.WriteLine($"EXEC | {order.OrderId} | {executedQty}@{order.Price}");
            Console.WriteLine("TRADE PRINTED");

            if (order.Quantity <= 0)
                _orders.Remove(order.OrderId);
        }

        private void OnReduce(MarketEvent evt)
        {
            if (!_orders.TryGetValue(evt.OrderId, out var order))
                return;

            var reducedQty = evt.Quantity;
            order.Quantity -= reducedQty;

            var book = order.Side == Side.Bid ? _bids : _asks;
            book[order.Price] -= reducedQty;

            if (book[order.Price] <= 0)
                book.Remove(order.Price);

            Console.WriteLine($"REDUCE | {order.OrderId} | {reducedQty}@{order.Price}");

            if (order.Quantity <= 0)
                _orders.Remove(order.OrderId);
        }

        private void OnDelete(long orderId)
        {
            if (!_orders.TryGetValue(orderId, out var order))
                return;

            var book = order.Side == Side.Bid ? _bids : _asks;
            book[order.Price] -= order.Quantity;

            if (book[order.Price] <= 0)
                book.Remove(order.Price);

            _orders.Remove(orderId);

            Console.WriteLine($"DELETE | {orderId}");
        }

        private void OnReplace(MarketEvent evt)
        {
            if (evt.OldOrderId == null) return;
            OnDelete(evt.OldOrderId.Value);

            var addEvt = new MarketEvent
            {
                Type = MarketDataEventType.Add,
                OrderId = evt.OrderId,
                Side = evt.Side,
                Price = evt.Price,
                Quantity = evt.Quantity
            };

            OnAdd(addEvt);

            Console.WriteLine($"REPLACE | {evt.OldOrderId} → {evt.OrderId}");
        }

        public void PrintTop()
        {
            var bestBid = _bids.FirstOrDefault();
            var bestAsk = _asks.FirstOrDefault();

            Console.WriteLine(
                $"TOP: BID {bestBid.Value}@{bestBid.Key} | ASK {bestAsk.Value}@{bestAsk.Key}"
            );
        }
    }
}
