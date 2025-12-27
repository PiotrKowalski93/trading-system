using MarketData.Core;
using MarketData.Core.ITCH;
using System.Net.WebSockets;

namespace MarketData.Consumer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            var ws = new ClientWebSocket();
            await ws.ConnectAsync(new Uri("ws://localhost:5062/ws/orderEvents"), cts.Token);

            var lob = new LimitOrderBook();

            var buffer = new byte[256];

            while (true)
            {
                var result = await ws.ReceiveAsync(buffer, cts.Token);

                if(result != null)
                {
                    var evt = ItchSerializer.Deserialize(buffer);
                    lob.Apply(evt);
                }
            }
        }
    }
}
