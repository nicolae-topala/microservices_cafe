using MassTransit;
using MicroservicesCafe.Products.Infrastructure;
using MicroservicesCafe.Shared.BuildingBlocks.BackgroundJobs;
using Quartz;

namespace MicroservicesCafe.Products.API.Infrastructure.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection RegisterRabbitMqService(this IServiceCollection services, IConfiguration configuration)
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

        public static IServiceCollection RegisterQuartzService(this IServiceCollection services)
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob<ProductsDbContext>));

            services.AddQuartz(configure =>
            {
                configure
                .AddJob<ProcessOutboxMessagesJob<ProductsDbContext>>(jobKey)
                .AddTrigger(
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));
            });

            return services;
        }
    }
}
