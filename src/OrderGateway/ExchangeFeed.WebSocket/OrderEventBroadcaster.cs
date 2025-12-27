using System.Net.WebSockets;

namespace ExchangeFeed.WebSockets
{
    public class OrderEventBroadcaster : IBroadcaster
    {
        private readonly List<WebSocket> _clients = new();
        private readonly object _lock = new();

        public void Register(WebSocket ws)
        {
            lock (_lock)
                _clients.Add(ws);
        }

        public async Task BroadcastAsync(byte[] payload)
        {
            List<WebSocket> snapshot;
            lock (_lock)
                snapshot = _clients.ToList();

            foreach (var ws in snapshot)
            {
                if (ws.State == WebSocketState.Open)
                {
                    await ws.SendAsync(
                        payload,
                        WebSocketMessageType.Binary,
                        true,
                        CancellationToken.None
                    );
                }
            }
        }
    }

    public interface IBroadcaster
    {
        void Register(WebSocket ws);
        Task BroadcastAsync(byte[] payload);
    }
}
