using PokemonReviewApp.Services.OwnerServices;
using Serilog;
using Castle.DynamicProxy;

namespace PokemonReviewApp.Aspect
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddProxiedServices(this IServiceCollection services)
        {
            var proxyGenerator = new ProxyGenerator();
            services.AddSingleton<LoggingInterceptor>();

            // Proxy ile sarmalanacak servisler
            services.AddScoped<IOwnerService>(provider =>
            {
                var implementation = provider.GetRequiredService<OwnerService>();
                var interceptor = provider.GetRequiredService<LoggingInterceptor>();
                return proxyGenerator.CreateInterfaceProxyWithTarget<IOwnerService>(implementation, interceptor);
            });

            return services;
        }
    }
}
