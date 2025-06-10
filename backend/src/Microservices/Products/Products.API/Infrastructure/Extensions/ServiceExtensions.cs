using MassTransit;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using Products.Application.Abstractions;
using Products.Infrastructure;
using Quartz;
using Shared.BuildingBlocks.BackgroundJobs;

namespace Products.API.Infrastructure.Extensions;

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
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob<>));

            configure.AddJob<ProcessOutboxMessagesJob<IProductsDbContext>>(jobKey)
                    .AddTrigger(trigger =>
                        trigger
                            .ForJob(jobKey)
                            .WithSimpleSchedule(schedule =>
                                schedule
                                    .WithIntervalInSeconds(3)
                                    .RepeatForever()));
        });

        services.AddQuartzHostedService();
        return services;
    }

    public static IServiceCollection RegisterOpenIddict(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        services.AddOpenIddict()
            .AddValidation(options =>
            {
                var issuer = configuration["OpenIddict:Issuer"]
                    ?? throw new InvalidOperationException("Issuer not found.");

                var audiesnces = configuration["OpenIddict:Audiences"]
                    ?? throw new InvalidOperationException("Audiences not found.");

                options.SetIssuer(issuer);
                options.AddAudiences(audiesnces);

                var encryptionKey = configuration["OpenIddict:EncryptionKey"]
                    ?? throw new InvalidOperationException("EncryptionKey not found.");
                options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(encryptionKey)));

                options.UseSystemNetHttp();
                options.UseAspNetCore();
            });

        services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        services.AddAuthorization();

        return services;
    }
}
