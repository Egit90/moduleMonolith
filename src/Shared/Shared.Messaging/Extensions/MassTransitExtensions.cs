using System.Reflection;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Messaging.Extensions;

public static class MassTransitExtensions
{
    public static IServiceCollection AddMassTransitWithAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();
            cfg.SetInMemorySagaRepositoryProvider();
            cfg.AddConsumers(assemblies);
            cfg.AddSagaStateMachines(assemblies);
            cfg.AddSagas(assemblies);
            cfg.AddActivities(assemblies);

            cfg.UsingInMemory((context, config) =>
            {
                config.ConfigureEndpoints(context);
            });

        });

        return services;
    }
}
