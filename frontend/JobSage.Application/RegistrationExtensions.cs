using Microsoft.Extensions.DependencyInjection;
using JobSage.Application.Jobs.Commands;
namespace JobSage.Application
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // Replace 'JobCommandHandler' with a valid type from the assembly, e.g., 'CreateJobCommandHandler'
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateJobCommand>());
            return services;
        }
    }
}
