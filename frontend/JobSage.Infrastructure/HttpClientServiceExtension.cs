using JobSage.Application.Interfaces;
using JobSage.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobSage.Infrastructure
{
    public static class HttpClientServiceExtension
    {
        public static IServiceCollection AddHttpClientServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            
            var chatServiceUrl = configuration["ChatServiceURL"];
            if (string.IsNullOrEmpty(chatServiceUrl))
                throw new ArgumentNullException("ChatServiceURL", "ChatServiceURL configuration value is missing.");
            
            services.AddHttpClient<IChatAgentService, ChatAgentService>(c => c.BaseAddress = new Uri(chatServiceUrl));

            return services;
        }
    }
}
