var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Fusion")
    .AddHeaderPropagation();

builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("GraphQL-Preflight");
    options.Headers.Add("Authorization");
});

builder.Services.ConfigureHttpClientDefaults(httpClientBuilder => httpClientBuilder.AddHeaderPropagation());

builder.Services
    .AddFusionGatewayServer()
    .ConfigureFromFile(
        "./gateway.fgp",
        watchFileForUpdates: true);

builder.Services
    .AddGraphQLServer();

var app = builder.Build();

app.UseHeaderPropagation();

app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
