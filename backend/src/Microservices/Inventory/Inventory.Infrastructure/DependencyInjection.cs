using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.BuildingBlocks.Interceptors;
using System.Reflection;

namespace Inventory.Infrastructure;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDbContext(configuration);

        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        return services;
    }

    private static IServiceCollection RegisterDbContext(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("InventoryDbConnectionString");

        services.AddPooledDbContextFactory<InventoryDbContext>((sp, options) =>
        {
            var inteceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()
                ?? throw new InvalidOperationException("OutboxMessagesInterceptor is not configured.");

            options.UseSqlServer(connectionString)
                .AddInterceptors(inteceptor);
        });

        // Abstract DbContext creation from the DbContextFactory
        // Transient is used in order to prevent sharing of same instances across multiple consumer
        // Default DbContext created with the factory is also Transient
        services.AddTransient<IInventoryDbContext>(provider =>
        {
            var factory = provider.GetRequiredService<IDbContextFactory<InventoryDbContext>>();
            return factory.CreateDbContext();
        });

        return services;
    }
}
