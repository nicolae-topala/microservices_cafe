using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;

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

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        var issuer = builder.Configuration["OpenIddict:Issuer"]
            ?? throw new InvalidOperationException("Issuer not found.");

        var audiesnces = builder.Configuration["OpenIddict:Audiences"]
            ?? throw new InvalidOperationException("Audiences not found.");

        var encryptionKey = builder.Configuration["OpenIddict:EncryptionKey"]
            ?? throw new InvalidOperationException("EncryptionKey not found.");

        options.SetIssuer(issuer);
        options.AddAudiences(audiesnces);

        options.AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(encryptionKey)));

        if (!builder.Environment.IsDevelopment())
        {
            options.UseSystemNetHttp();
        }
        else
        {
            options.UseSystemNetHttp()
                   .ConfigureHttpClientHandler(handler =>
                   {
                       handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
                   });
        }
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer();

var app = builder.Build();

app.UseHeaderPropagation();

app.UseAuthentication();
app.UseAuthorization();

//app.MapGraphQL()
//    .RequireAuthorization();
app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
