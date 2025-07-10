using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;

namespace Shared.BuildingBlocks.WebHost;

public static class WebHostExtensions
{
    public static IApplicationBuilder MigrateDbContext<TContext>(this IApplicationBuilder builder,
         Action<TContext, IServiceProvider>? seeder = null)
         where TContext : DbContext
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        var logger = services.GetRequiredService<ILogger<TContext>>();

        TContext context;
        try
        {
            var factory = services.GetService<IDbContextFactory<TContext>>();
            if (factory != null)
            {
                context = factory.CreateDbContext();
            }
            // If no factory is registered(Auth API), try to get the context directly
            else
            {
                context = services.GetRequiredService<TContext>();
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while trying to access {DbContext}: {Exception}", 
                typeof(TContext).Name, ex.Message);
            throw;
        }

        try
        {
            logger.LogInformation("Migrating database associated with context {DbContextName}",
                typeof(TContext).Name);

            var retries = 15;
            var retry = Policy.Handle<Exception>()
                .WaitAndRetry(
                    retries,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                    (exception, timeSpan, retryCount, ctx) =>
                    {
                        logger.LogWarning(exception,
                            "[{Prefix}] Exception {ExceptionType} with message {Message} detected on attempt {Retry} of {Retries}",
                            nameof(TContext), exception.GetType().Name, exception.Message, retryCount, retries);
                    });

            var result = retry.ExecuteAndCapture(() =>
            {
                context.Database.SetCommandTimeout(3600);
                try
                {
                    logger.LogInformation("Executing migration for context {DbContextName}",
                        typeof(TContext).Name);
                    context.Database.Migrate();
                    logger.LogInformation("Migration completed for context {DbContextName}",
                        typeof(TContext).Name);
                }
                catch (Exception migrateEx)
                {
                    logger.LogError(migrateEx, "Migration failed for context {DbContextName}",
                        typeof(TContext).Name);
                    throw;
                }

                try
                {
                    if (seeder != null)
                    {
                        logger.LogInformation("Seeding database associated with context {DbContextName}",
                            typeof(TContext).Name);
                        seeder(context, services);
                    }
                }
                catch (Exception seedEx)
                {
                    logger.LogError(seedEx, "An error occurred during seeding for context {DbContextName}",
                        typeof(TContext).Name);
                    throw;
                }
            });

            if (result.Outcome == OutcomeType.Failure)
            {
                logger.LogError(result.FinalException, "Failed to migrate database after {RetryCount} retries for context {DbContextName}",
                    retries, typeof(TContext).Name);
            }
            else
            {
                logger.LogInformation("Successfully migrated and seeded database associated with context {DbContextName}",
                    typeof(TContext).Name);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled error occurred while migrating the database for context {DbContextName}",
                typeof(TContext).Name);
        }

        return builder;
    }
}