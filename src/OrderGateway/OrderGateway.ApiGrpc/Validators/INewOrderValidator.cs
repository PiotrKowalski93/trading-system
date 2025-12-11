using OrderGateway.ApiGrpc.Protos;

namespace OrderGateway.ApiGrpc.Validators
{
    public interface INewOrderValidator
    {
        NewOrderValidationResult Validate(NewOrderRequest order);
    }
}
