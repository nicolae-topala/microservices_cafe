using MicroservicesCafe.Products.API.Infrastructure.Extensions;
using MicroservicesCafe.Products.Application;
using MicroservicesCafe.Products.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .AddTypes()
    .AddMutationConventions(applyToAllMutations: true);
    //.AddDefaultTransactionScopeHandler();

builder.Services
    .RegisterApplicationServices(builder.Configuration)
    .RegisterInfrastructureServices(builder.Configuration)
    .RegisterRabbitMqService(builder.Configuration)
    .RegisterQuartzService();

var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
