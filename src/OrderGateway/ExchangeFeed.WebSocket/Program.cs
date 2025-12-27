using System.Net.WebSockets;

namespace ExchangeFeed.WebSockets
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();
            builder.Services.AddSwaggerGen();

            builder.Services.AddSingleton<IBroadcaster, OrderEventBroadcaster>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwagger();
                app.UseSwaggerUI(options => // UseSwaggerUI is called only in Development.
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseWebSockets();
            app.Map("/ws/orderEvents", async context =>
            {
                if (!context.WebSockets.IsWebSocketRequest)
                    return;

                var broadcaster = context.RequestServices.GetRequiredService<IBroadcaster>();

                using var ws = await context.WebSockets.AcceptWebSocketAsync();
                broadcaster.Register(ws);

                // keep-alive loop
                var buffer = new byte[1];
                while (ws.State == WebSocketState.Open)
                    await ws.ReceiveAsync(buffer, CancellationToken.None);
            });

            app.MapControllers();

            app.Run();
        }
    }
}
