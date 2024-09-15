using MicroservicesCafe.Products.API.Infrastructure.Extensions;
using MicroservicesCafe.Products.Application;
using MicroservicesCafe.Products.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddAuthorization()
    .AddTypes()
    .AddMutationConventions(applyToAllMutations: true);
//.AddDefaultTransactionScopeHandler();

builder.Services
    .RegisterApplicationServices(builder.Configuration)
    .RegisterInfrastructureServices(builder.Configuration)
    .RegisterRabbitMqService(builder.Configuration)
    .RegisterQuartzService()
    .RegisterOpenIddict(builder.Configuration, builder.Environment);

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapGraphQL();

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
