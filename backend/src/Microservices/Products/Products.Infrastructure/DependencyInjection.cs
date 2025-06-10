using Products.Application.Abstractions;
using Shared.BuildingBlocks.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductsDbConnectionString");

        services.AddPooledDbContextFactory<ProductsDbContext>((sp, options) =>
        {
            var inteceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(connectionString)
                .AddInterceptors(inteceptor);
        });

        // Abstract DbContext creation from the DbContextFactory
        // Transient is used in order to prevent sharing of same instances across multiple consumer
        // Default DbContext created with the factory is also Transient
        services.AddTransient<IProductsDbContext>(provider =>
        {
            var factory = provider.GetRequiredService<IDbContextFactory<ProductsDbContext>>();
            return factory.CreateDbContext();
        });

        return services;
    }
}
