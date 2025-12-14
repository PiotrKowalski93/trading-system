namespace OrderGateway.ApiGrpc.Redis
{
    public interface IStartupCacheLoader
    {
        Task LoadAsync();
    }
}
