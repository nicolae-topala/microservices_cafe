using User.API.Infrastructure.Extensions;
using User.Application;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services
    .RegisterOpenIddict(builder.Configuration, builder.Environment);

builder.Services
    .RegisterApplicationServices(builder.Configuration);


builder.Services.AddHeaderPropagation(options =>
{
    options.Headers.Add("Authorization");
});


builder.Services.AddHttpClient("UserMicroservice", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["OpenIddict:Issuer"]);
})
.AddHeaderPropagation(options =>
{
    options.Headers.Add("Authorization");
});

builder.Services.ConfigureHttpClientDefaults(httpClientBuilder => httpClientBuilder.AddHeaderPropagation());

builder
    .AddGraphQL()
    .AddAuthorization()
    .AddTypes();

var app = builder.Build();

app.UseHeaderPropagation();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
