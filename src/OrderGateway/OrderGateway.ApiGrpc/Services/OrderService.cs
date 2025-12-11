using Grpc.Core;
using OrderGateway.ApiGrpc.Protos;
using OrderGateway.ApiGrpc.Repositories;
using OrderGateway.ApiGrpc.Validators;

namespace OrderGateway.ApiGrpc.Services
{
    public class OrderService : Orders.OrdersBase
    {
        private readonly IInMemoryOrderRepository _orderRepository;
        private readonly INewOrderValidator _orderValidator;

        public OrderService(IInMemoryOrderRepository orderRepository, INewOrderValidator orderValidator)
        {
            _orderRepository = orderRepository;
            _orderValidator = orderValidator;
        }

        public override Task<NewOrderResponse> NewOrder(NewOrderRequest request, ServerCallContext context)
        {
            // Validate Order
            var validationResult = _orderValidator.Validate(request);
            if (validationResult.Failure)
            {
                return Task.FromResult(new NewOrderResponse
                {
                    GatewayOrderId = request.ClientOrderId,
                    Status = "Rejected",
                    Reason = validationResult.FailedMessage
                });
            }

            // Save order in in-memory store + XADD to redis
            // ...
            _orderRepository.AddOrUpdate

            return Task.FromResult(new NewOrderResponse
            {
                GatewayOrderId = request.ClientOrderId,
                Status = "Accepted"
            });

            //return base.NewOrder(request, context);
        }
    }
}
