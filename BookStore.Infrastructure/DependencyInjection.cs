using Microsoft.Extensions.DependencyInjection;

namespace BookStore.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Infrastructure currently has no runtime registrations; AutoMapper configured in Application
            return services;
        }
    }
}
