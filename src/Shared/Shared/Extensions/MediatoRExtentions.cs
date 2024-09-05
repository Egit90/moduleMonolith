using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;

namespace Shared.Extensions;

public static class MediatoRExtensions
{
    public static IServiceCollection AddMediatorWithAssemblies(this IServiceCollection services, params Assembly[] assemblyList)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(assemblyList);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
            config.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });

        services.AddValidatorsFromAssemblies(assemblyList);
        return services;
    }
}