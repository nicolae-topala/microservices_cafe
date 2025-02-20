using Gateway.API.Settings;
using HotChocolate.Execution;

namespace Gateway.API.Infrastructure.Extensions;

public static class AppBuilderExtensions
{
    // Using docker compose will save the file to the container 
    public static async void SaveGraphQlSchemaToFile(this IApplicationBuilder app, IConfiguration configuration)
    {
        var schema = await app.ApplicationServices.GetSchemaAsync();

        //var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();

        try
        {
            var settings = configuration.GetSection("GraphQL").Get<GraphQlSettings>()!;

            if (!settings.IsGenerateSchema)
                return;

            var path = settings.PathToSchema;
            //logger.LogInformation("Saving GraphQL Schema to: {Path}", path);
            File.WriteAllText(path, schema.ToString());
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            //logger.LogError(ex, "An error occurred while saving the GraphQL schema");
        }
    }
}
