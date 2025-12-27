using MarketData.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace MarketData.Consumer
{
    public class MarketDataConsumer
    {
        private readonly ChannelReader<MarketEvent> _reader;
        private readonly LimitOrderBook _lob;

        public MarketDataConsumer(
            ChannelReader<MarketEvent> reader,
            LimitOrderBook lob)
        {
            _reader = reader;
            _lob = lob;
        }

        public async Task StartAsync(CancellationToken ct)
        {
            await foreach (var evt in _reader.ReadAllAsync(ct))
            {
                _lob.Apply(evt);
                _lob.PrintTop();
            }
        }
    }
}
