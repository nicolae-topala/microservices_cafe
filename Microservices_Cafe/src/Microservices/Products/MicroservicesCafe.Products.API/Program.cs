using HotChocolate.Data;
using MicroservicesCafe.Products.API.Infrastructure.Extensions;
using MicroservicesCafe.Products.Application;
using MicroservicesCafe.Products.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddGraphQLServer()
    .RegisterDbContext<ProductsDbContext>(DbContextKind.Pooled)
    .AddTypes();

builder.Services
    .RegisterApplicationServices(builder.Configuration)
    .RegisterInfrastructureServices(builder.Configuration)
    .RegisterRabbitMqService(builder.Configuration)
    .RegisterQuartzService();

var app = builder.Build();

app.MapGraphQL();

app.RunWithGraphQLCommands(args);
