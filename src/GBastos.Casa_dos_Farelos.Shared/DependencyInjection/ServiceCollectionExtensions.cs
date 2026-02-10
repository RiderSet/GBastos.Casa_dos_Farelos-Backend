using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace GBastos.Casa_dos_Farelos.Shared.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddShared(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining(typeof(ServiceCollectionExtensions));
        return services;
    }
}
