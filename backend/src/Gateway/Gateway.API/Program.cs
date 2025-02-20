using Gateway.API.Infrastructure.Extensions;

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

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()
            ?? throw new InvalidOperationException("Allowed Origins not found.");

        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

app.UseCors();

app.UseHeaderPropagation();

app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
