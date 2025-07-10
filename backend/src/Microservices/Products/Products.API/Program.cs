using Products.API.Infrastructure.Extensions;
using Products.Application;
using Products.Infrastructure;
using Shared.BuildingBlocks.Elasticsearch;
using Shared.BuildingBlocks.WebHost;

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

app.MigrateDbContext<ProductsDbContext>();

app.MapGraphQL();

await app.InitializeElasticsearchIndicesAsync(ElasticIndex.Products);

await app.RunWithGraphQLCommandsAsync(args)
    .ConfigureAwait(false);
