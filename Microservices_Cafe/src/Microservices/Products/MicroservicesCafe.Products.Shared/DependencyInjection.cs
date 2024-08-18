using Mapster;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroservicesCafe.Products.Shared;

public static class DependencyInjection
    {
        public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

        public static IServiceCollection RegisterMapsterConfiguration(this IServiceCollection services)
        {
            //TypeAdapter<>
            //    .NewConfig()
            //    .Map();

            TypeAdapterConfig.GlobalSettings.Scan(currentAssembly);

            return services;
        }
    }

