using FluentValidation;
using GBastos.Casa_dos_Farelos.Application.Validators.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace GBastos.Casa_dos_Farelos.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var assembly = Assembly.GetExecutingAssembly();


            // ------------------ MEDIATR ------------------
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(assembly);
            });


            // ------------------ FLUENT VALIDATION ------------------
            services.AddValidatorsFromAssembly(assembly);


            // ------------------ PIPELINE BEHAVIORS ------------------
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            // (opcional futuro)
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));


            return services;
        }
    }
}
