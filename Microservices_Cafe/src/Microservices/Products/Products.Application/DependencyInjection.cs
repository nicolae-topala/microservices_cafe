using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Products.Application
{
    public static class DependencyInjection
    {
        public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        public static IServiceCollection RegisterApplicationServices(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(currentAssembly));

            services.AddValidatorsFromAssembly(currentAssembly);

            services.RegisterMapsterConfiguration();

            return services;
        }

        public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
        {
            var config = new TypeAdapterConfig();
            config.Scan(currentAssembly);

            services.AddSingleton(config);
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
