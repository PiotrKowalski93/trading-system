using OrderGateway.Core.Instruments;

namespace OrderGateway.ApiGrpc.Caches
{
    public interface IInMemoryInstrumentCache
    {
        InstrumentMetadata? Get(string symbol);
        bool Exists(string symbol);
        void AddOrUpdate(InstrumentMetadata meta);
    }
}
