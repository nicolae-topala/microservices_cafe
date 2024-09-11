using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient("Fusion");

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

        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services
    .AddGraphQLServer()
    .AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//app.MapGraphQL()
//    .RequireAuthorization();
app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
