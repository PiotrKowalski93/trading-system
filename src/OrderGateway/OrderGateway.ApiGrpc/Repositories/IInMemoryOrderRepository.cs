using OrderGateway.ApiGrpc.Models;

namespace OrderGateway.ApiGrpc.Repositories
{
    public interface IInMemoryOrderRepository
    {
        Order? GetOrder(string id);
        IEnumerable<Order> GetAll();
        void AddOrUpdate(Order order);
        bool Delete(string id);
    }
}
