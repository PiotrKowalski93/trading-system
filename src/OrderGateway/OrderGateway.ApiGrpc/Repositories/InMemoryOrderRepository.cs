using OrderGateway.ApiGrpc.Models;

namespace OrderGateway.ApiGrpc.Repositories
{
    public class InMemoryOrderRepository : IOrderRepository
    {
        public void AddOrUpdate(Order order)
        {
            throw new NotImplementedException();
        }

        public bool Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Order> GetAll()
        {
            throw new NotImplementedException();
        }

        public Order? GetOrder(string id)
        {
            throw new NotImplementedException();
        }
    }

    public interface IOrderRepository
    {
        Order? GetOrder(string id);
        IEnumerable<Order> GetAll();
        void AddOrUpdate(Order order);
        bool Delete(string id);
    }
}
