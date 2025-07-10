using Elastic.Clients.Elasticsearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Products.Application.Abstractions;
using Shared.Abstractions;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.Interceptors;
using System.Reflection;

namespace Products.Infrastructure;

public static class DependencyInjection
{
    public static readonly Assembly currentAssembly = typeof(DependencyInjection).Assembly;

    public static IServiceCollection RegisterInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.RegisterDbContext(configuration);
        services.RegisterElasticClient(configuration);

        services.AddScoped<IElasticsearchService, ElasticsearchService>();
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        return services;
    }

    private static IServiceCollection RegisterDbContext(this IServiceCollection services, 
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ProductsDbConnectionString");

        services.AddPooledDbContextFactory<ProductsDbContext>((sp, options) =>
        {
            var inteceptor = sp.GetService<ConvertDomainEventsToOutboxMessagesInterceptor>()
                ?? throw new InvalidOperationException("OutboxMessagesInterceptor is not configured.");

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

    private static IServiceCollection RegisterElasticClient(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Elasticsearch");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Elasticsearch connection string is not configured.");
        }

        services.AddSingleton(sp =>
        {
            var settings = new ElasticsearchClientSettings(new Uri(connectionString));
            return new ElasticsearchClient(settings);
        });

        return services;
    }
}
