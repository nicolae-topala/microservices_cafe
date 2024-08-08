using MassTransit;

namespace MicroservicesCafe.Product.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.SetKebabCaseEndpointNameFormatter();

                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration["RabbitMq:ConnectionString"], configuration["RabbitMq:VirtualHost"], h =>
                    {
                        h.Username(configuration["RabbitMq:Username"]);
                        h.Password(configuration["RabbitMq:Password"]);
                    });

                    configurator.ConfigureEndpoints(context);
                });
            });

            return services;
        }

    }
}
