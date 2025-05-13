using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Price.Application.Abstractions;
using Shared.BuildingBlocks.Interceptors;
using System.Reflection;

namespace Price.Infrastructure;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PriceDbConnectionString");

        services.AddPooledDbContextFactory<PriceDbContext>((sp, options) =>
        {
            var inteceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>();

            options.UseSqlServer(connectionString);
            //.AddInterceptors(inteceptor);
        });

        // Abstract DbContext creation from the DbContextFactory
        // Transient is used in order to prevent sharing of same instances across multiple consumer
        // Default DbContext created with the factory is also Transient
        services.AddTransient<IPriceDbContext>(provider =>
        {
            var factory = provider.GetRequiredService<IDbContextFactory<PriceDbContext>>();
            return factory.CreateDbContext();
        });

        return services;
    }
}