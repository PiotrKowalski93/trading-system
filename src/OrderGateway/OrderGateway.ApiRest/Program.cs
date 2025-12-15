using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OrderGateway.ApiRest.Redis;
using Prometheus;
using StackExchange.Redis;

namespace OrderGateway.ApiRest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddOpenTelemetry()
            //    .WithMetrics(opt =>
            //        opt
            //            .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(""))
            //            .AddMeter("")
            //            .AddAspNetCoreInstrumentation()
            //            .AddRuntimeInstrumentation()
            //            .AddProcessInstrumentation()
            //            .AddOtlpExporter(opt =>
            //            {
            //                //TODO: Move to config .json
            //                opt.Endpoint = new Uri("localhost:9090");
            //            })
            //        );

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //TODO: Move to .json config
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect("localhost:6379")
            );
            builder.Services.AddSingleton<IBrokerRulesRedisWriter, BrokerRulesRedisWriter>();
            builder.Services.AddSingleton<IInstrumentMetadataRedisWriter, InstrumentMetadataRedisWriter>();

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

            app.UseHttpMetrics();     // metryki HTTP (latency, statusy, RPS)
            app.MapMetrics("/metrics"); // endpoint /metrics

            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
