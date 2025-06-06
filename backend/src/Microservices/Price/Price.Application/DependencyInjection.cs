﻿using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Abstractions.Behaviors;
using System.Reflection;

namespace Price.Application;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection RegisterApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(currentAssembly);
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

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