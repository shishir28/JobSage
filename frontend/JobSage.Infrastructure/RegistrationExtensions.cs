using JobSage.Domain.Repositories;
using JobSage.Infrastructure.Persistence.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace JobSage.Infrastructure
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services)
        {
            services.AddScoped<IJobRepository, JobRepository>();
            services.AddScoped<IContractorRepository, ContractorRepository>();

            return services;
        }
    }
}
