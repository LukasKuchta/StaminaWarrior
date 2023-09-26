using Backend.Application.Behaviour;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Application;

public static class BackendApplicationLayerInitializer
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(BackendApplicationAssembly.Instance);

        services.AddMediatR(configuraton =>
        {
            configuraton.RegisterServicesFromAssembly(BackendApplicationAssembly.Instance);
            //configuraton.AddOpenBehavior(typeof(LoggingBehaviour<,>));
            configuraton.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            configuraton.AddOpenBehavior(typeof(UnitOfWorkBehaviour<,>));
        });

        return services;
    }
}
