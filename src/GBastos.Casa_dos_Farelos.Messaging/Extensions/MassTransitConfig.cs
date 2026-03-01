using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.BuildingBlocks.Messaging.Extensions;

public static class MassTransitConfig
{
    public static IServiceCollection AddMessageBroker(
        this IServiceCollection services,
        IConfiguration config)
    {
        object value = services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(config["RabbitMQ:Host"]);
                cfg.ConfigureEndpoints(ctx);
            });
        });

        return services;
    }
}