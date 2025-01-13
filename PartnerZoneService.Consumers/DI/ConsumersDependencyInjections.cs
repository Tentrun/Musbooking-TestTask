using EventBus.RMQ.Extensions;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerZoneService.Consumers.Consumers.Implementations;
using PartnerZoneService.Consumers.Definitions;

namespace PartnerZoneService.Consumers.DI;

public static class ConsumersDependencyInjections
{
    public static void AddPartnerZoneConsumers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddRmqEventBus(configuration, regCfg =>
        {
            regCfg.AddConsumer<PartnerZoneConsumer>(typeof(PartnerZoneConsumerDefinition));
        }, cfg =>
        {
            cfg.UseMessageRetry(r =>
            {
                r.Intervals(TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(60),
                    TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(180),
                    TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15),
                    TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(60),
                    TimeSpan.FromHours(2));
                r.Handle<InvalidOperationException>();
            });
        });
    }
}
