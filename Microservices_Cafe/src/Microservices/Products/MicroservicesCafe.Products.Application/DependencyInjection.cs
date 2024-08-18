using FluentValidation;
using Mapster;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroservicesCafe.Products.Application
{
    public static class DependencyInjection
    {
        public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services, 
            IConfiguration configuration)
        {
            services.AddMediatR(configuration =>
                configuration.RegisterServicesFromAssembly(currentAssembly));

            services.AddValidatorsFromAssembly(currentAssembly);

            return services;
        }

        public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
        {
            //TypeAdapter<>
            //    .NewConfig()
            //    .Map();

            TypeAdapterConfig.GlobalSettings.Scan(currentAssembly);

            return services;
        }
    }
}
