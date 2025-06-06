using Price.API.Infrastructure.Extensions;
using Price.Application;
using Price.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddFiltering()
    .AddSorting()
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
