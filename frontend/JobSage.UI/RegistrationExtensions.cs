using JobSage.UI.Services;

namespace JobSage.UI
{
    public static class RegistrationExtensions
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            var baseAddress = new Uri(configuration["ApiSettings:GatewayAddress"]);
            services.AddHttpClient<IJobService, JobService>(c => c.BaseAddress = baseAddress);
            services.AddHttpClient<IContractorService, ContractorService>(c =>
                c.BaseAddress = baseAddress
            );
            services.AddHttpClient<IChatService, ChatService>(c => c.BaseAddress = baseAddress);
            // services.AddScoped<INotificationService, NotificationService>();
            return services;
        }
    }
}
