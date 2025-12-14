namespace OrderGateway.ApiGrpc.Caches
{
    public interface IStartupCacheLoader
    {
        Task LoadAsync();
    }
}
