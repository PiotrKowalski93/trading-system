using OrderGateway.ApiGrpc.Models;
using System.Collections.Concurrent;

namespace OrderGateway.ApiGrpc.Repositories
{
    public class InMemoryOrderRepository : IInMemoryOrderRepository
    {
        private readonly ConcurrentDictionary<string, Order> _orders = new();

        public void AddOrUpdate(Order order)
        {
            _orders.AddOrUpdate(order.ClientOrderId, 
                order, 
                (key, oldVal) => order);
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            if (_orders.TryRemove(id, out _)) return true;
            return false;
        }

        public IEnumerable<Order> GetAll()
        {
            return _orders.Values;
        }

        public Order? GetOrder(string orderId)
        {
            if(_orders.TryGetValue(orderId, out var order)) return order;
            return null;
        }
    }
}
